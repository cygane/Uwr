#heurystyka : zwraca najgorsza odleglosc komandosa do celu

import heapq

class State:
    def __init__(self, moves, positions, g_cost):
        self.moves = moves
        self.positions = tuple(sorted(positions)) 
        self.g_cost = g_cost  

    def __lt__(self, other):
        return (self.g_cost + self.heuristic()) < (other.g_cost + other.heuristic())

    def heuristic(self):
        return max(goal_distance[y][x] for y, x in self.positions) 

board = []
goal_distance = []
y_size = 0
x_size = 0
starting_positions = set()
goal_positions = set()

def read_input():
    global board, y_size, x_size, starting_positions, goal_positions
    with open("zad_input.txt", "r") as file:
        lines = file.readlines()

    y_size = len(lines)
    x_size = len(lines[0].strip())
    board = [[0] * x_size for _ in range(y_size)]
    
    for row, line in enumerate(lines):
        for col, char in enumerate(line.strip()):
            if char == 'B':
                starting_positions.add((row, col))
                goal_positions.add((row, col))
            elif char == 'G':
                goal_positions.add((row, col))
            elif char == 'S':
                starting_positions.add((row, col))
            elif char == '#':
                board[row][col] = 1  

def print_result(result):
    with open("zad_output.txt", "w") as file:
        file.write(result + "\n")

def safe(position):
    y, x = position
    return 0 <= x < x_size and 0 <= y < y_size

def available(position):
    y, x = position
    return safe(position) and board[y][x] == 0

directions = "LRUD"
dirs = [(0, -1), (0, 1), (-1, 0), (1, 0)]

def move(pos, dir):
    new_pos = (pos[0] + dirs[dir][0], pos[1] + dirs[dir][1])
    return new_pos if available(new_pos) else pos

def make_move(positions, dir):
    return tuple(sorted(move(pos, dir) for pos in positions)) 

def calc_dist_from_goals(start):
    global goal_distance
    vis = [[False] * x_size for _ in range(y_size)]
    queue = [(start, 0)]  
    
    vis[start[0]][start[1]] = True
    while queue:
        position, distance = queue.pop(0)
        goal_distance[position[0]][position[1]] = min(goal_distance[position[0]][position[1]], distance)

        for dir in range(4):
            new_position = move(position, dir)
            if not vis[new_position[0]][new_position[1]]:
                queue.append((new_position, distance + 1))
                vis[new_position[0]][new_position[1]] = True

def calc_distance_array():
    global goal_distance
    goal_distance = [[float('inf')] * x_size for _ in range(y_size)]
    for pos in goal_positions:
        calc_dist_from_goals(pos)

def dist(positions):
    return max(goal_distance[y][x] for y, x in positions)

def win(positions):
    return all(pos in goal_positions for pos in positions)

def solve():
    read_input()
    calc_distance_array()

    starting_state = State("", starting_positions, 0)  # 0 ruchow na poczatku
    pq = [starting_state]
    heapq.heapify(pq)

    visited = set()
    visited.add(starting_state.positions)  

    while pq:
        state = heapq.heappop(pq)

        if win(state.positions):
            print_result(state.moves)
            return

        for i in range(4):
            new_positions = make_move(state.positions, i)
            new_state = State(state.moves + directions[i], new_positions, state.g_cost + 1)

            if new_positions not in visited:  
                heapq.heappush(pq, new_state)
                visited.add(new_positions)

solve()
