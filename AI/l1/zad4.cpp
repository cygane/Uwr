#include <iostream>
#include <fstream>
#include <vector>
using namespace std;

// przegladamy okno d miejsc i sprawdzamy, dla ktorego d miejsc wywolamy najmniej zmian

int opt_dist(const vector<int>& nums, int d) {
    int n = nums.size();
    int ones = 0;
    for (int bit : nums) {
        if (bit == 1) {
            ones++;
        }
    }

    if(d > n){
        return -1;
    }
    if (d == 0) { 
        return ones;
    }

    int min_changes = INT_MAX;

    for (int i = 0; i <= n - d; i++) {
        int changes = 0;
        for (int j = 0; j < d; j++) {
            if (nums[i + j] != 1){
                changes++;
            }
        }
        int to_del = ones + changes - d;
        min_changes = min(min_changes, changes + to_del);
    }
    return min_changes;
}

int main() {
    vector<int> lst;
    lst.push_back(0);
    lst.push_back(0);
    lst.push_back(1);
    lst.push_back(0);
    lst.push_back(0);
    lst.push_back(0);
    lst.push_back(1);
    lst.push_back(0);
    lst.push_back(0);
    lst.push_back(0);

    cout << opt_dist(lst, 5) << endl; //3
    cout << opt_dist(lst, 4) << endl; //4
    cout << opt_dist(lst, 3) << endl; //3
    cout << opt_dist(lst, 2) << endl; //2
    cout << opt_dist(lst, 1) << endl; //1
    cout << opt_dist(lst, 0) << endl; //2
    cout << opt_dist(lst, 11) << endl; //-1
}
