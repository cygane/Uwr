// Julia Cygan 338682
#include <iostream>
#include <cstdlib>
#include <cstring>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netinet/ip_icmp.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <cassert>
#include <poll.h>
#include <chrono>
using namespace std;
using namespace std::chrono;

const int WAIT = 1000;
const int ECHO_SEND = 3;
const int MAX_TTL = 31;

struct res{
    string reply = "";
    int avg = 0;
    int count_replies = 0;
};

u_int16_t compute_icmp_checksum(const void *buff, int length){
    const u_int16_t* ptr = (const u_int16_t*)buff;
    u_int32_t sum = 0;
    assert (length % 2 == 0);
    for(; length > 0; length -= 2){
        sum += *ptr++;
    }
    sum = (sum >> 16U) + (sum & 0xffffU);
    return (u_int16_t)(~(sum + (sum >> 16U)));
}

void ERROR(const char* str){
    fprintf(stderr, "%s: %s\n", str, strerror(errno));
    exit(EXIT_FAILURE);
}


void print_as_bytes (unsigned char* buff, ssize_t length){
    for (ssize_t i = 0; i < length; i++, buff++){
        printf("%.2x ", *buff);
    }
}

icmp create(uint16_t& identifier){
    struct icmp header;
    header.icmp_type = ICMP_ECHO;
    header.icmp_code = 0;
    header.icmp_hun.ih_idseq.icd_id = identifier;
    header.icmp_hun.ih_idseq.icd_seq = 1;
    header.icmp_cksum = 0;
    header.icmp_cksum = compute_icmp_checksum (
    (u_int16_t*)&header, sizeof(header));

    return header;
}

void send(int& sockfd, sockaddr_in &recipient, int& ttl, icmp& header){
    setsockopt(sockfd, IPPROTO_IP, IP_TTL, &ttl, sizeof(ttl));
    if(setsockopt(sockfd, IPPROTO_IP, IP_TTL, &ttl, sizeof(ttl)) < 0){
        ERROR("Błąd ustawiania TTL");
    }

    for(int i = 0; i < ECHO_SEND; i++){
        ssize_t bytes_sent = sendto(
        sockfd,
        &header,
        sizeof(header),
        0,
        (struct sockaddr*)&recipient,
        sizeof(recipient)
        );

        if(bytes_sent < 0){
            ERROR("Błąd wysyłania pakietu");
        }
    }
}

res receive_reply(int& sockfd, time_point<system_clock>& start, uint16_t& identifier){
    struct pollfd ps;
    ps.fd = sockfd;
    ps.events = POLLIN;
    ps.revents = 0;

    res result;
    time_point<system_clock> end;

    while(result.count_replies < ECHO_SEND){
        int ready = poll(&ps, 1, WAIT);
        if(ready > 0 && (ps.revents & POLLIN)){
            struct sockaddr_in sender;
            socklen_t sender_len = sizeof(sender);
            u_int8_t buffer[IP_MAXPACKET];

            ssize_t packet_len = recvfrom(sockfd, buffer, IP_MAXPACKET, 0,(struct sockaddr*)&sender, &sender_len);
            if(packet_len < 0){
                ERROR("Błąd odbierania pakietu");
            }

            end = system_clock::now();
            duration<int, milli> dur = duration_cast<duration<int, milli> >(end - start);

            char sender_ip[INET_ADDRSTRLEN];
            inet_ntop(AF_INET, &(sender.sin_addr), sender_ip, sizeof(sender_ip));

            if (inet_ntop(AF_INET, &(sender.sin_addr), sender_ip, sizeof(sender_ip)) == nullptr) {
                ERROR("Błąd konwersji adresu IP");
            }

            struct ip* ip_header = (struct ip*) buffer;
            ssize_t ip_header_len = 4 * ip_header->ip_hl;
            struct icmp* icmp_header = (struct icmp*) (buffer + ip_header_len);
            struct ip* orig_ip_header = (struct ip*) (buffer + ip_header_len + 8);
            struct icmp* orig_icmp_header = (struct icmp*) ((u_int8_t*)orig_ip_header + orig_ip_header->ip_hl * 4);

            if(orig_icmp_header->icmp_hun.ih_idseq.icd_id == identifier || icmp_header->icmp_hun.ih_idseq.icd_id == identifier){
                result.reply = sender_ip;
                result.avg += dur.count();
                result.count_replies++;

                if(result.count_replies == ECHO_SEND){
                    break;  
                }
            }
        } 
        else {
            break; 
        }
    }
    return result;
}


int main(int argc, char **argv){
    if(argc != 2){
        ERROR("Brak adresu IP");
    }

    int sockfd = socket(AF_INET, SOCK_RAW, IPPROTO_ICMP);
    if (sockfd < 0) {
        ERROR("Błąd tworzenia gniazda");
    }

    uint16_t identifier = getpid();
    icmp header = create(identifier);

    struct sockaddr_in recipient;
    memset (&recipient, 0, sizeof(recipient));
    recipient.sin_family = AF_INET;
    if(inet_pton(AF_INET, argv[1], &recipient.sin_addr) <= 0){
        ERROR("Niepoprawny adres IP");
    }

    
    for(int ttl = 1; ttl < MAX_TTL; ttl++){
        send(sockfd,recipient,ttl,header);

        time_point<system_clock> start = system_clock::now();
        res send_result = receive_reply(sockfd, start, identifier);

        if(send_result.reply == ""){
            cout<<ttl<<". *\n";
        }
        else{
            cout<<ttl<<". "<<send_result.reply<<" ";
            if(send_result.count_replies == ECHO_SEND){
                cout<<send_result.avg/ECHO_SEND<<"ms\n";
            }
            else{
                cout<<"???\n";
            }
        }

        if (send_result.reply == argv[1]){
            break;
        }
    }
    close(sockfd);
    return 0;
}