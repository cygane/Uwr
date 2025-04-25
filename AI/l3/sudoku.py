def V(i, j):
    return 'V%d_%d' % (i, j)

def domains(Vs):
    return [q + ' in 1..9' for q in Vs]

def all_different(Qs):
    return 'all_distinct([' + ', '.join(Qs) + '])'

def get_column(j):
    return [V(i, j) for i in range(9)]

def get_row(i):
    return [V(i, j) for j in range(9)]

def horizontal():
    return [all_different(get_row(i)) for i in range(9)]

def vertical():
    return [all_different(get_column(j)) for j in range(9)]

def block(i, j):
    return [V(i + di, j + dj) for di in range(3) for dj in range(3)]

def blocks():
    return [all_different(block(i, j)) for i in [0,3,6] for j in [0,3,6]]

def print_constraints(Cs, indent, d):
    position = indent
    print((indent - 1) * ' ', end=' ', file=out_file)
    for c in Cs:
        print(c + ',', end=' ', file=out_file)
        position += len(c)
        if position > d:
            position = indent
            print(file=out_file)
            print(indent * ' ', end=' ', file=out_file)


def sudoku(assigments):
    variables = [V(i, j) for i in range(9) for j in range(9)]

    print(':- use_module(library(clpfd)).', file=out_file)
    print('solve([' + ', '.join(variables) + ']) :- ', file=out_file)

    cs = domains(variables) + vertical() + horizontal() + blocks()
    for i, j, val in assigments:
        cs.append('%s #= %d' % (V(i, j), val))

    print_constraints(cs, 4, 70),
    print(file=out_file)
    print('    labeling([ff], [' + ', '.join(variables) + ']).', file=out_file)
    print(file=out_file)
    print(':- solve(X), write(X), nl.', file=out_file)


if __name__ == "__main__":
    out_file = open("zad_output.txt", "w", encoding='utf8')
    input_file = open("zad_input.txt", encoding='utf8').readlines()

    row = 0
    triples = []

    for x in input_file:
        x = x.strip()
        if len(x) == 9:
            for col in range(9):
                if x[col] != '.':
                    triples.append((row, col, int(x[col])))
            row += 1
    sudoku(triples)

    out_file.close()
