# zachlanne ruchy sa wynonywane losowo, jednak jest zabezpieczenie zeby sie nie cofac, 
# jesli poszlam w dol w poprzednim ruchu to w nastepnym nie ide w gore

# jesli mam wiecej komandosow niz potrzeba to wtedy robie losowe ruchy o robie tak 100 iteracji 
# i zwiekszam liczbe komandosow na zwiekszenie ppb rozwiazania
# wykonuje do 80 losowych ruchow, chyba ze liczba komandosow spadnie szybciej

import random

board_walls = list()
board_start = set()
board_goal = set()

def read_board():
    row = 0
    with open("zad_input.txt", "r") as input_file:
        for line in input_file:
            line = (line.rstrip('\r')).rstrip('\n')
            if len(line) == 0:
                continue

            col = 0
            temp_walls = list()
            for i in line:
                if i == '#':
                    temp_walls.append(True)
                else:
                    temp_walls.append(False)

                if i == 'G':
                    board_goal.add((row, col))
                elif i == 'S':
                    board_start.add((row, col))
                elif i == 'B':
                    board_goal.add((row, col))
                    board_start.add((row, col))
                col += 1

            board_walls.append(temp_walls)
            row += 1

directions = [(-1, 0), (1, 0), (0, -1), (0, 1)]
directions_names = "UDLR"

def sum_positions(a, b):  
    return (a[0] + b[0], a[1] + b[1])

def move_safe(move):   
    return (not board_walls[move[0]][move[1]]) and \
           (move[0] < len(board_walls) and move[0] >= 0) and \
           (move[1] < len(board_walls[0]) and move[0] >= 0)

def make_move(state, direction):
    new_state = set()
    for commando in state:
        new_move = sum_positions(commando, direction)
        if move_safe(new_move):
            new_state.add(new_move)
        else:
            new_state.add(commando)
    return new_state

def goal_reached(state):   
    for commando in state:
        if commando not in board_goal:
            return False
    return True

def make_random_moves(start_state, count=50, commandos_alive=2):
    last_random_move = -1
    path = ""
    for i in range(count):
        random_move = random.randint(0, 3)
        while (last_random_move == 1 and random_move == 0) or (last_random_move == 3 and random_move == 2):
            random_move = random.randint(0, 3)

        start_state = make_move(start_state, directions[random_move])
        last_random_move = random_move
        path += directions_names[random_move]

        if len(start_state) <= commandos_alive:
            break

    return start_state, path

def hash_state(state):      
    state = sorted(state)
    state_hash = 0
    multiplier = 1
    for i in state:
        state_hash += multiplier * ((i[0] * len(board_walls)) + i[1])
        multiplier *= len(board_walls) * len(board_walls[0])

    return state_hash

def bfs(start_state):
    queue = []
    visited = []
    queue.append((start_state, ""))
    visited.append(hash_state(start_state))
    
    while queue:
        current_state, path = queue.pop(0)
        if goal_reached(current_state):
            return current_state, path

        for d in range(4):
            new_state = make_move(current_state, directions[d])
            if hash_state(new_state) not in visited:
                queue.append((new_state, path + directions_names[d]))
                visited.append(hash_state(new_state))

    return False, ""

def print_board(state):
    for row in range(len(board_walls)):
        for col in range(len(board_walls[row])):
            if board_walls[row][col]:
                print("#", end='')
            elif (row, col) in state and (row, col) in board_goal:
                print("B", end='')
            elif (row, col) in state:
                print("S", end='')
            elif (row, col) in board_goal:
                print("G", end='')
            else:
                print(" ", end='')

        print()

read_board()

result = False
path1 = ""
path2 = ""    
while result == False:
    state = board_start

    max_commandos = 2
    iterations = 0
    while len(state) > max_commandos:
        state, path1 = make_random_moves(board_start, 80)
        iterations += 1
        if iterations == 100:
            max_commandos = 3

    result, path2 = bfs(state)

print(path1 + path2)
with open("zad_output.txt", "w") as output_file:
    output_file.write(path1 + path2)
