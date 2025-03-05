import numpy as np
import random

# obrazek trzymamy jako 0,1
# row_val i col_val to d dla wierszy i kolumn
# row_correct i col_correct to poprawne wiersze
# naprawianie kolumny oraz wiersza robimy liczac za pomoca sum prefiksowych i wybieramy to d miejsc, ktore bedzie mialo najmniej zmian
# zapisujemy gdzie sie te d miejsc konczy, zeby potem zmienic obrazek

R = 7
C = 7
picture = np.array([[0] * R] * C)
row_val = [0] * R
col_val = [0] * C
row_correct = [0] * R
col_correct = [0] * C
max_iter = 5000


def reset():
    row_correct = [0] * R
    col_correct = [0] * C
    picture = random_picture()
    return row_correct, col_correct, picture


def print_picture(picture=picture):
    output = open("zad5_output.txt", "w")
    for i in range(R):
        for j in range(C):
            if picture[i][j]:
                output.write('#')
            else:
                output.write('.')
        output.write('\n')
    output.close()


def random_picture():
    return np.random.choice([1, 0], size=(R, C))


def check_row(row):
    sum = 0
    for i in range(0,C):
        if picture[row][i]:
            sum += 1

    if sum != row_val[row]: return False
    return True


def check_col(col):
    sum = 0
    for i in range(0, R):
        if picture[i][col]:
            sum += 1

    if sum != col_val[col]: return False
    return True


def check_rows():
    for i in range(R):
        if not check_row(i): return False
    return True


def check_cols():
    for i in range(C):
        if not check_col(i): return False
    return True


def incorrect_row():
    rows = []
    for row in range(R):
        if not check_row(row):
            rows.append(row)
    if len(rows) > 0: return np.random.choice(rows)
    return -1


def incorrect_col():
    cols = []
    for col in range(C):
        if not check_col(col):
            cols.append(col)
    if len(cols) > 0: return np.random.choice(cols)
    return -1

def fix_row(row):
    prefix = [0] * R
    len = row_val[row]
    max_corr = 0
    id_end = 0

    if picture[row][0]:
        prefix[0] = 1
    for i in range(1, len):
        prefix[i] = prefix[i - 1] + picture[row][i]
    max_corr = prefix[len - 1]
    id_end = len - 1

    for i in range(len, R):
        if i > 0: prefix[i] = prefix[i - 1]
        if picture[row][i - len]: prefix[i] -= 1
        if picture[row][i]: prefix[i] += 1

        if prefix[i] > max_corr:
            max_corr = prefix[i]
            id_end = i

    for i in range(0, id_end - len + 1): picture[row][i] = False
    for i in range(id_end - len + 1, id_end + 1): picture[row][i] = True
    for i in range(id_end + 1, R): picture[row][i] = False


def fix_col(col):
    prefix = [0] * C
    len = col_val[col]
    max_corr = 0
    id_end = 0

    if picture[0][col]:
        prefix[0] = 1
    for i in range(1, len):
        prefix[i] = prefix[i - 1] + picture[i][col]
    max_corr = prefix[len - 1]
    id_end = len - 1

    for i in range(len, C):
        if i > 0: prefix[i] = prefix[i - 1]
        if picture[i - len][col]: prefix[i] -= 1
        if picture[i][col]: prefix[i] += 1

        if prefix[i] > max_corr:
            max_corr = prefix[i]
            id_end = i

    for i in range(0, id_end - len + 1): picture[i][col] = False
    for i in range(id_end - len + 1, id_end + 1): picture[i][col] = True
    for i in range(id_end + 1, C): picture[i][col] = False


def try_to_solve():
    for t in range(max_iter):
        row = incorrect_row()
        if row != -1: fix_row(row)

        col = incorrect_col()
        if col != -1: fix_col(col)

        if check_cols() and check_rows(): return True, picture
    return False, picture


def read_input():
    with open("zad5_input.txt", "r") as input:
        s = input.readline().split()
        R = (int)(s[0])
        C = (int)(s[1])
        for i in range(R): row_val[i] = (int)(input.readline())
        for i in range(C): col_val[i] = (int)(input.readline())
        return R, C


R, C = read_input()

solved = False
while not solved:
    row_correct, col_correct, picture = reset()
    solved, picture = try_to_solve()

print_picture(picture)
