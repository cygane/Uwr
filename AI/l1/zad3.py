import random
from collections import Counter

#figurant nigdy nie osiagnie pokera, ani strita, ani koloru, ale za to zawsze osiagnie pare
#jesli figurant i blotkarz maja ten sam uklad to zawsze wygra figurant

def losuj(talia):
    return random.sample(talia, 5)

def licz(hand):
    counter = Counter(hand)
    values = sorted(counter.values(), reverse=True)
    
    if len(counter) == 1:  #poker
        return 7
    elif values[0] == 4:  #kareta
        return 6
    elif values[0] == 3 and values[1] == 2:  #full
        return 5
    elif values[0] == 3:  #trojka
        return 3
    elif values[0] == 2 and values[1] == 2:  #dwie pary
        return 2
    elif values[0] == 2:  #para
        return 1
    else:  #wysoka karta
        return 0

def symuluj(rundy, blotkarz_talia=None):
    figurant_talia = [11, 12, 13, 14] * 4  
    blotkarz_talia = blotkarz_talia or list(range(2, 11)) * 4  
    
    wygrane = 0
    for _ in range(rundy):
        figurant = losuj(figurant_talia)
        blotkarz = losuj(blotkarz_talia)
        
        if licz(blotkarz) > licz(figurant):
            wygrane += 1
    
    return wygrane / rundy


ppb = symuluj(1000)
print(f"Szansa Blotkarza na zwycięstwo: {ppb:.2%}")


talia = [8, 9, 10] * 4  
ppb_2 = symuluj(1000, talia)
print(f"Szansa Blotkarza na zwycięstwo (ulepszona talia): {ppb_2:.2%}")
