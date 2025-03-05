words = set()
max_word_len = 0

# slowa zapisuje do slownika
# znajduje najlepsze rozlozenie slow za pomoca sprawdzenia coraz wiekszych slow w slowniku az do max dlugosci slowa ze slownika
# zapisuje gdzie dane slowo sie zaczyna w tablicy father


def load_dictionary(filename):
    global max_word_len
    with open(filename, 'rb') as words_file:
        for line in words_file:
            word = line.decode('utf-8').strip()
            if word:
                words.add(word)
                max_word_len = max(max_word_len, len(word))


def find_best_partition(line):
    dp = [0] * (len(line) + 1)
    father = [-1] * (len(line) + 1)
    father[0] = 0

    for r in range(1, min(max_word_len, len(line) + 1)):
        if line[0:r] in words:
            dp[r] = r ** 2
            father[r] = 0

    for l in range(1, len(line)):
        if father[l] == -1:
            continue
        for r in range(l + 1, min(l + max_word_len, len(line) + 1)):
            if line[l:r] in words:
                cost = dp[l] + (r - l) ** 2
                if dp[r] < cost:
                    dp[r] = cost
                    father[r] = l

    star_id = len(line)
    result = ""
    while star_id != 0:
        result = line[father[star_id]:star_id] + ' ' + result
        star_id = father[star_id]

    return result.strip()



def process_input(input_filename, output_filename):
    with open(input_filename, "rb") as tadeusz, open(output_filename, "wb") as output:
        for line in tadeusz:
            line = line.decode("utf-8").strip()
            partition = find_best_partition(line)
            output.write(partition.encode("utf-8"))
            output.write(b'\n')


def main():
    load_dictionary('word_for_ai.txt')

    global max_word_len
    max_word_len += 5

    process_input("zad2_input.txt", "zad2_output.txt")



if __name__ == "__main__":
    main()
