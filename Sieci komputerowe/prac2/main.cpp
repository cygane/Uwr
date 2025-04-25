#include <arpa/inet.h>
#include <errno.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <vector>
#include <iostream>
#include <sstream>
#include <thread>
#include <chrono>
#include <map>
#include <string>
#include <algorithm>
#include <set>
#include <fstream>
#include <assert.h>

const uint32_t INF = 0xFFFFFFFF;
const int inf = 16;
const int MAX_TTL = 3;
const int PORT = 54321;
const int MAX_BUFFER_SIZE = 512;
const int TIME_OUT = 60;
const int TIME_STAMP = 15;

using namespace std;

struct NetworkData{
    string ip;
    string broadcast_ip;
    int distance;
    string via;
    bool directly_connected;
    int ttl;
    chrono::steady_clock::time_point last_received;
};

map<string, NetworkData> routing_table;
map<string, pair<string,int>> direct_networks;
vector<vector<int>> interface;
vector<string> interface_string;
int n;

bool is_valid_cidr(const string& cidr){
    size_t slash_pos = cidr.find('/');
    if(slash_pos == string::npos){
        return false;
    }
    
    string ip_part = cidr.substr(0, slash_pos);
    string mask_part = cidr.substr(slash_pos + 1);
    
    struct sockaddr_in sa;
    if(inet_pton(AF_INET, ip_part.c_str(), &(sa.sin_addr)) != 1){
        return false;
    }

    int mask_len = stoi(mask_part);
    if(mask_len < 0 || mask_len > 32){
        return false;
    }

    return true;
}

uint32_t ip_to_int(const string& ip){
    struct in_addr addr;
    inet_pton(AF_INET, ip.c_str(), &addr);
    return ntohl(addr.s_addr);
}

pair<string, int> parse_cidr(const string& cidr){
    size_t slash_pos = cidr.find('/');
    string network = cidr.substr(0, slash_pos);
    int mask = stoi(cidr.substr(slash_pos + 1));
    return {network, mask};
}

string cidr_to_mask(int cidr){
    uint32_t mask = (INF << (32 - cidr)) & INF;
    struct in_addr mask_addr;
    mask_addr.s_addr = htonl(mask);
    char str[INET_ADDRSTRLEN];
    inet_ntop(AF_INET, &mask_addr, str, INET_ADDRSTRLEN);
    return string(str);
}

bool is_in_network(const string& ip, const string& cidr){
    auto [network_ip, mask_len] = parse_cidr(cidr);

    uint32_t ip_int = ip_to_int(ip);
    uint32_t network_int = ip_to_int(network_ip);
    string mask_str = cidr_to_mask(mask_len);
    uint32_t mask_int = ip_to_int(mask_str);

    uint32_t network_ip_result = ip_int & mask_int;

    return network_ip_result == (network_int & mask_int);
}

void ERROR(const char* str){
    fprintf(stderr, "%s: %s\\n", str, strerror(errno));
    exit(EXIT_FAILURE);
}

vector<int> parse_ip(const string& ip_str){
    vector<int> ip(5);  
    stringstream ss(ip_str);
    string octet;

    size_t slash_pos = ip_str.find('/');
    string ip_part = ip_str.substr(0, slash_pos);
    string mask_part = ip_str.substr(slash_pos + 1);

    stringstream ip_ss(ip_part);

    for(int i = 0; i < 4; i++){
        getline(ip_ss, octet, '.');
        ip[i] = stoi(octet);
    }

    ip[4] = stoi(mask_part);  
    return ip;
}

vector<int> calculate_broadcast(const vector<int>& ip){
    uint32_t ip_bin = (ip[0] << 24) | (ip[1] << 16) | (ip[2] << 8) | ip[3];
    uint32_t mask_bin = (INF << (32 - ip[4])) & INF;
    uint32_t broadcast_bin = ip_bin | ~mask_bin;

    vector<int> broadcast(4);
    broadcast[0] = (broadcast_bin >> 24) & 0xFF;
    broadcast[1] = (broadcast_bin >> 16) & 0xFF;
    broadcast[2] = (broadcast_bin >> 8) & 0xFF;
    broadcast[3] = broadcast_bin & 0xFF;

    return broadcast;
}

vector<int> calculate_network(const vector<int>& ip){
    uint32_t ip_bin = (ip[0] << 24) | (ip[1] << 16) | (ip[2] << 8) | ip[3];
    uint32_t mask_bin = (INF << (32 - ip[4])) & INF;
    uint32_t network_bin = ip_bin & mask_bin;

    vector<int> network(4);
    network[0] = (network_bin >> 24) & 0xFF;
    network[1] = (network_bin >> 16) & 0xFF;
    network[2] = (network_bin >> 8) & 0xFF;
    network[3] = network_bin & 0xFF;

    return network;
}

void print_table(){
    for(auto [key, data] : routing_table){
        cout<<key;
        if(data.distance == inf){
            cout<<" unreachable";
        }
        else{
            cout<<" distance "<<data.distance;
        }

        if(data.directly_connected){
            cout<<" connected directly"<<"\n";
        }
        else {
            cout<<" via "<<data.via<<"\n";
        }
    }
    cout<<"\n";
}

void send_data(int sock_fd){
    while(true){
        print_table();
        set<string> to_del;
        for(auto& [key, data]: direct_networks){
            auto& data_in_rounting = routing_table[key];
            auto now = chrono::steady_clock::now();
            auto duration = chrono::duration_cast<chrono::seconds>(now - data_in_rounting.last_received);
            if(duration.count() > TIME_OUT){
                for(auto& [dest_key, entry] : routing_table){
                    if(is_in_network(entry.via, data_in_rounting.ip)){
                        entry.distance = inf;
                    } 
                }
            }
            for(auto& [neighbor_ip, neighbor_data] : routing_table){
                if(neighbor_data.distance != inf || (neighbor_data.distance == inf && neighbor_data.ttl > 0)){
                    if(neighbor_data.distance == inf){
                        neighbor_data.ttl--;
                    }
                    uint8_t udp[9];
                    string ip_str = neighbor_ip.substr(0, neighbor_ip.find('/')); 

                    struct in_addr ip_struct;
                    inet_pton(AF_INET, ip_str.c_str(), &ip_struct);
                    memcpy(&udp[0], &ip_struct, 4); 

                    int mask_len = stoi(neighbor_ip.substr(neighbor_ip.find('/') + 1)); 
                    udp[4] = mask_len;

                    uint32_t dist = neighbor_data.distance; 
                    dist = htonl(dist);
                    memcpy(&udp[5], &dist, 4);
                    struct sockaddr_in addr{};
                    addr.sin_family = AF_INET;
                    addr.sin_port = htons(PORT);
                    inet_pton(AF_INET, data_in_rounting.broadcast_ip.c_str(), &addr.sin_addr);

                    ssize_t sent = sendto(sock_fd, udp, 9, 0, (struct sockaddr*)&addr, sizeof(addr));

                    if(sent != 9){
                        data_in_rounting.distance = inf;
                    }
                    if(sent == 9){
                        data_in_rounting.via = "";
                        data_in_rounting.directly_connected = true;
                        data_in_rounting.ttl = MAX_TTL;
                        data_in_rounting.distance = direct_networks[data_in_rounting.ip].second;
                    }
                    assert(fflush(stdout) == 0);
                }

                if(neighbor_data.distance == inf && neighbor_data.ttl == 0){
                    if(direct_networks.find(neighbor_ip) == direct_networks.end()){
                        to_del.insert(neighbor_ip);
                    }
                } 
            }
        }
        for(string ip : to_del){
            routing_table.erase(ip);
        }

        this_thread::sleep_for(chrono::seconds(TIME_STAMP));  
    }
}

void receive(int sock_fd){
    struct sockaddr_in sender;
    socklen_t sender_len = sizeof(sender);
    uint8_t buffer[MAX_BUFFER_SIZE];

    while(true){
        ssize_t datagram_len = recvfrom(sock_fd, buffer, sizeof(buffer), 0, (struct sockaddr*)&sender, &sender_len);
        if(datagram_len < 0){
            ERROR("recvfrom error");
        }

        if(datagram_len != 9){
            ERROR("datagram_len error");
            continue;
        }

        char sender_ip_str[INET_ADDRSTRLEN];
        inet_ntop(AF_INET, &(sender.sin_addr), sender_ip_str, sizeof(sender_ip_str));

        bool is_from_self = false;
        for(const auto& iface : interface_string){
            string ip_part = iface.substr(0, iface.find('/'));
            if(ip_part == string(sender_ip_str)){
                is_from_self = true;
                break;
            }
        }
        if(is_from_self){
            continue;  
        }

        uint8_t ip_bytes[4];
        memcpy(ip_bytes, &buffer[0], 4);
 
        uint8_t mask_recv = buffer[4];
        char ip_key[32];
        snprintf(ip_key, sizeof(ip_key), "%u.%u.%u.%u/%u", ip_bytes[0], ip_bytes[1], ip_bytes[2], ip_bytes[3], mask_recv);
        

        uint32_t dist_recv;
        memcpy(&dist_recv, &buffer[5], 4);
        dist_recv = ntohl(dist_recv);

        string router_ip = string(sender_ip_str);
        auto it = routing_table.find(ip_key);

        bool found = false;
        int my_dist_to_sender = inf;
        for(auto& [key, iface] : routing_table){
            if(is_in_network(router_ip, key)){
                my_dist_to_sender = iface.distance;
                iface.last_received = chrono::steady_clock::now();
                found = true;
                break;
            }
        }

        if(found){
            vector<int> interface_recv = parse_ip(ip_key);
            vector<int> bcast_ip_recv = calculate_broadcast(interface_recv);
            string broad_recv = to_string(bcast_ip_recv[0]) + "." + to_string(bcast_ip_recv[1]) + "." +
                            to_string(bcast_ip_recv[2]) + "." + to_string(bcast_ip_recv[3]);

            NetworkData data_recv;
            data_recv.ip = ip_key;
            data_recv.broadcast_ip = broad_recv;
            data_recv.distance = (dist_recv == inf || my_dist_to_sender == inf) ? inf : dist_recv + my_dist_to_sender;
            data_recv.via = router_ip;
            data_recv.directly_connected = false;
            data_recv.ttl = MAX_TTL;
            it = routing_table.find(ip_key);

            if(data_recv.distance < inf){
                auto sender_it = find_if(routing_table.begin(), routing_table.end(), [&](const auto& entry){
                    return is_in_network(router_ip, entry.first);
                });

                bool sender_alive = (sender_it != routing_table.end() && sender_it->second.distance < inf);

                if(sender_alive){
                    if(it == routing_table.end()){
                        routing_table[ip_key] = data_recv;
                    } 
                    else{
                        bool is_better = (it->second.distance > data_recv.distance);
                        bool is_same_via = (it->second.via == data_recv.via);

                        if(is_better || is_same_via){
                            routing_table[ip_key] = data_recv;
                        }
                    }
                }
            }
            else{
                if(it->second.via != ""){
                    it->second.distance = inf;
                }
            }
        }

    }
}

int main(){
    cin>>n;

    interface.resize(n, vector<int>(5, 0));
    interface_string.resize(n);

    for(int i = 0; i < n; i++){
        string d;
        int dist_network;

        cin>>interface_string[i]>>d>>dist_network;
       
        if(!is_valid_cidr(interface_string[i])){
            ERROR("wrong CIDR address");
        }

        interface[i] = parse_ip(interface_string[i]);

        vector<int> bcast_ip = calculate_broadcast(interface[i]);
        vector<int> network_ip = calculate_network(interface[i]);

        string net = to_string(network_ip[0]) + "." + to_string(network_ip[1]) + "." +
             to_string(network_ip[2]) + "." + to_string(network_ip[3]) + "/" + to_string(interface[i][4]);

        string broad = to_string(bcast_ip[0]) + "." + to_string(bcast_ip[1]) + "." +
                       to_string(bcast_ip[2]) + "." + to_string(bcast_ip[3]);

        direct_networks[net] = make_pair(interface_string[i], dist_network);

        NetworkData data;
        data.ip = net;
        data.broadcast_ip = broad;
        data.distance = dist_network;
        data.via = "";
        data.directly_connected = true;
        data.ttl = MAX_TTL;
        data.last_received = chrono::steady_clock::now();

        routing_table[net] = data;
    }

    int sock_fd = socket(AF_INET, SOCK_DGRAM, 0);
    if(sock_fd < 0){
        ERROR("socket error");
    }

    int broadcastEnable = 1;
    setsockopt(sock_fd, SOL_SOCKET, SO_BROADCAST, &broadcastEnable, sizeof(broadcastEnable));

    struct sockaddr_in server_address;
    memset(&server_address, 0, sizeof(server_address));
    server_address.sin_family      = AF_INET;
    server_address.sin_port        = htons(PORT);
    server_address.sin_addr.s_addr = htonl(INADDR_ANY);

    if(::bind(sock_fd, (struct sockaddr*)&server_address, sizeof(server_address)) < 0){
        ERROR("bind error");
    }

    thread send_thread(send_data, sock_fd);
    thread receive_thread(receive, sock_fd);

    send_thread.join(); 
    receive_thread.join();
    close(sock_fd);

    return 0;
}