# Wytłumaczyć różnicę między SRP a ISP.
SRP mówi, że klasa powinna mieć tylko jedną odpowiedzialność, czyli powinna robić tylko jedną rzecz, i powinna być zmieniana z tylko jednego powodu.

ISP mówi, że klient nie powinien być zmuszany do implementowania interfejsów, których nie używa. Interfejsy powinny być wąskie, specyficzne dla danej funkcjonalności, a nie obejmować szerokiego zakresu metod, które mogą nie być potrzebne w danym kontekście.

SRP (Single Responsibility Principle) odnosi się do klas i mówi, że każda klasa powinna mieć tylko jedną odpowiedzialność, czyli tylko jeden powód do zmiany. Jeśli klasa ma wiele odpowiedzialności, staje się trudna w utrzymaniu i testowaniu.

ISP (Interface Segregation Principle) odnosi się do interfejsów i mówi, że interfejsy nie powinny być zmuszane do implementowania metod, których nie potrzebują. Zamiast tworzyć jeden duży interfejs, lepiej stworzyć wiele mniejszych, bardziej wyspecjalizowanych interfejsów.