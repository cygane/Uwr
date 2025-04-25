def V(i,j):
    return 'V_%d_%d' % (i,j)

def domains(Vs):
    return [q + ' in 0..1' for q in Vs]

# A + 2B + 3C ̸= 2
def v(R, C):
    constraint = []
    for i in range(1, R - 1):
        for j in range(C):
            constraint.append(V(i - 1, j) + " + 2 * " + V(i, j) + " + 3 * " + V(i + 1, j) + " #\= 2")
    return constraint

def h(R, C):
    constraint = []
    for i in range(R):
        for j in range(1, C - 1):
            constraint.append(V(i, j - 1) + " + 2 * " + V(i, j) + " + 3 * " + V(i, j + 1) + " #\= 2")
    return constraint

#liczenie wszystkich zakazanych wzorcow
#komorki 2x2 jako a b
#                 c d i robimy a + 2b + 4c + 8d
def forbidden(R, C):
    constraint = []
    values = [6, 14, 9, 13, 11, 7]      
    for i in range(R - 1):
        for j in range(C - 1):
            for value in values:
                constraint.append(V(i, j) + " + 2 * " + V(i, j + 1) + " + 4 * " + V(i + 1, j) + " + 8 * " + V(i + 1, j + 1) + " #\= " + str(value))
    return constraint

def v_sums(R, C):
    constraint = []
    for j in range(C):
        s = ""
        for i in range(R):
            s += V(i, j)
            if i != R - 1:
                s += " + "
        s += " #= " + str(cols[j])
        constraint.append(s)
    return constraint

def h_sums(R, C):
    constraint = []
    for i in range(R):
        s = ""
        for j in range(C):
            s += V(i, j)
            if j != C - 1:
                s += " + "
        s += " #= " + str(rows[i])
        constraint.append(s)
    return constraint

def add_a(assigments):
    constraint = []
    for i, j, val in assigments:
        constraint.append( '%s #= %d' % (V(i,j), val) )
    return constraint

def write(s):
    output.write(s)

def writeln(s):
    output.write(s + '\n')

def print_constraints(Cs, indent, d):
    position = indent
    write (indent * ' ')
    for c in Cs:
        write (c + ', ')
        position += len(c)
        if position > d:
            position = indent
            writeln ('')
            write (indent * ' ')

def storms(rows, cols, assigments):
    writeln(':- use_module(library(clpfd)).')

    R = len(rows)
    C = len(cols)

    variables = [V(i,j) for i in range(R) for j in range(C)]

    cs = domains(variables) + v(R, C) + h(R, C) + forbidden(R, C) + v_sums(R, C) + h_sums(R, C) + add_a(assigments)

    writeln('solve([' + ', '.join(variables) + ']) :- ')

    print_constraints(cs, 4, 70)
    
    writeln('    labeling([ff], [' +  ', '.join(variables) + ']).' )
    writeln('')
    writeln(":- tell('prolog_result.txt'), solve(X), write(X), nl, told.")

txt = open('zad_input.txt').readlines()
output = open('zad_output.txt', 'w')

rows = (list(map(int, txt[0].split())))
cols = (list(map(int, txt[1].split())))
triples = []

for i in range(2, len(txt)):
    if txt[i].strip():
        triples.append(list(map(int, txt[i].split())))

storms(rows, cols, triples)            
        
