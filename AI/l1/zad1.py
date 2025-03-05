from collections import deque

# oznaczenia: W - bialy krol, w - biala wieza, B - czarny krol
# uzywam bfs to obliczenia najmniejsze liczby ruchow do mata
# kazdy stan przepisuje na unikalny kod, zeby mozna bylo oznaczyc jako odwiedzony w bfs
# utrzymuje tablice father, to pozniejszego backtracka dla trybu debug

L = 8
N = 4 * (L * L) * (L * L) * (L * L) + 5
WHITE, BLACK = True, False
vis = [False] * N
father = [-1] * N

def generate_idx(turn, w, W, B):
    w = (w[0] - 1, w[1] - 1)
    W = (W[0] - 1, W[1] - 1)
    B = (B[0] - 1, B[1] - 1)
    return ((w[0] + L * w[1] + L * L * W[0] + L * L * L * W[1] + L * L * L * L * B[0] + L * L * L * L * L * B[1]) * 2 + (0 if turn else 1))

def generate_state(id):
    norm = id // 2
    w = (norm % L + 1, (norm // L) % L + 1)
    W = ((norm // (L * L)) % L + 1, (norm // (L * L * L)) % L + 1)
    B = ((norm // (L * L * L * L)) % L + 1, (norm // (L * L * L * L * L)) % L + 1)
    return (WHITE if id % 2 == 0 else BLACK, w, W, B)

def move_legal(p):
    return 1 <= p[0] <= L and 1 <= p[1] <= L

def safe_position(state):
    turn, w, W, B = state
    if turn == BLACK:
        for dx in [-1, 0, 1]:
            for dy in [-1, 0, 1]:
                if (W[0] + dx, W[1] + dy) == B:
                    return False
        moves = [(1, 0), (0, 1), (-1, 0), (0, -1)]
        for dx, dy in moves:
            for i in range(1, L + 1):
                new_w = (w[0] + i * dx, w[1] + i * dy)
                if not move_legal(new_w) or W == new_w:
                    break
                if B == new_w:
                    return False
    else:
        for dx in [-1, 0, 1]:
            for dy in [-1, 0, 1]:
                if (B[0] + dx, B[1] + dy) == W:
                    return False
    return True

def gen_possible_moves(n):
    state = generate_state(n)
    turn, w, W, B = state
    res = []
    if turn == BLACK:
        for dx in [-1, 0, 1]:
            for dy in [-1, 0, 1]:
                new_B = (B[0] + dx, B[1] + dy)
                if move_legal(new_B) and new_B != W and safe_position((turn, w, W, new_B)):
                    res.append(generate_idx(not turn, w, W, new_B))
    else:
        # if B == w:
        #     return res
        moves = [(1, 0), (0, 1), (-1, 0), (0, -1)]
        for dx, dy in moves:
            for i in range(1, L + 1):
                new_w = (w[0] + i * dx, w[1] + i * dy)
                if not move_legal(new_w) or B == new_w or W == new_w:
                    break
                res.append(generate_idx(not turn, new_w, W, B))
        for dx in [-1, 0, 1]:
            for dy in [-1, 0, 1]:
                new_W = (W[0] + dx, W[1] + dy)
                if move_legal(new_W) and new_W != w and new_W != B and safe_position((turn, w, new_W, B)):
                    res.append(generate_idx(not turn, w, new_W, B))
    return res

def bfs(start_id):
    global vis, father
    vis = [False] * N
    father = [-1] * N
    father[start_id] = start_id
    queue = deque([(start_id, 1)])
    while queue:
        now, now_dist = queue.popleft()
        if vis[now]:
            continue
        vis[now] = True
        new_moves = gen_possible_moves(now)
        state = generate_state(now)
        if state[0] == BLACK and not safe_position(state) and not new_moves:
            return now_dist, now
        for move in new_moves:
            if not vis[move]:
                father[move] = now
                queue.append((move, now_dist + 1))
    return -1, 0


def backtrack_path(end_id):
    path = []
    while father[end_id] != end_id:
        path.append(end_id)
        end_idx = father[end_id]
    path.append(end_id)
    path.reverse()

    print("\n--- Debug: Ścieżka do mata ---")
    for id in path:
        state = generate_state(id)
        print_state(state)
        print("---------------------------")


def print_state(state):
    turn, w, W, B = state
    board = [["." for _ in range(L)] for _ in range(L)]
    board[w[0] - 1][w[1] - 1] = "w"
    board[W[0] - 1][W[1] - 1] = "W"
    board[B[0] - 1][B[1] - 1] = "B"

    print("Ruch:", "Biały" if turn == WHITE else "Czarny")
    for row in reversed(board):
        print(" ".join(row))
    print()


def main(debug=False):
    with open("zad1_input.txt", "r") as input_file, open("zad1_output.txt", "w") as output_file:
        for line in input_file:
            parts = line.split()
            turn = WHITE if parts[0] == "white" else BLACK
            W = (int(parts[1][1]), ord(parts[1][0]) - ord('a') + 1)
            w = (int(parts[2][1]), ord(parts[2][0]) - ord('a') + 1)
            B = (int(parts[3][1]), ord(parts[3][0]) - ord('a') + 1)
            starting_idx = generate_idx(turn, w, W, B)
            res_steps, res_idx = bfs(starting_idx)
            if res_steps == -1:
                output_file.write("INF\n")
            else:
                output_file.write(f"{res_steps - 1}\n")
                if debug:
                    backtrack_path(res_idx)

if __name__ == "__main__":
    debug = False
    main(debug)