/**
 * @typedef {Object} Product
 * @property {number} id - identyfikator produktu
 * @property {string} name - nazwa produktu
 * @property {number} quantity - liczba sztuk do zakupienia
 * @property {Date} dueDate - data, do której produkt powinien być zakupiony
 * @property {boolean} isPurchased - status informujący, czy produkt został zakupiony
 * @property {number} [pricePerUnit] - opcjonalna cena za sztukę (dotyczy tylko zakupionych produktów)
 */

/** @type {Product[]} */
let productList = [];

/**
 * dodawanie nowego produktu do listy 
 *
 * @param {string} name - nazwa produktu
 * @param {number} quantity - liczba sztuk do zakupienia
 * @param {string} dueDateString - data zakupu 
 * @returns {Product} - dodany produkt
 */
function add_product(name, quantity, dueDateString){
    const newProduct = {
        id: Math.floor(Math.random() * 1_000_000),
        name,
        quantity,
        dueDate: new Date(dueDateString),
        isPurchased: false
    };
    productList.push(newProduct);
    return newProduct;
}

/**
 * usuwanie produktu z listy
 *
 * @param {number} id - identyfikator produktu
 * @returns {void} - funkcja nie zwraca wartości, modyfikuje produkty w miejscu
 */
function delete_product(id){
    productList = productList.filter(elem => elem.id !== id);
}

/**
 * edytuje nazwę produktu na podstawie id
 *
 * @param {number} id - identyfikator produktu
 * @param {string} newName - nowa nazwa produktu
 * @returns {void} - funkcja nie zwraca wartości, modyfikuje produkt w miejscu
 */
function edit_name(id, newName) {
    const elem = productList.find(elem => elem.id === id);
    if (elem) {
        elem.name = newName;
    }
}

/**
 * edytuje status produktu na podstawie id
 *
 * @param {number} id - identyfikator produktu
 * @param {boolean} status - nowy status produktu
 * @returns {void} - funkcja nie zwraca wartości, modyfikuje produkt w miejscu
 */
function edit_status(id, newStatus) {
    const elem = productList.find(elem => elem.id === id);
    if (elem) {
        elem.isPurchased = newStatus;
    }
}

/**
 * edytuje liczbe sztuk produktu na podstawie id
 *
 * @param {number} id - identyfikator produktu
 * @param {number} quantity - liczba sztuk
 * @returns {void} - funkcja nie zwraca wartości, modyfikuje produkt w miejscu
 */
function edit_quantity(id, newQuantity) {
    const elem = productList.find(elem => elem.id === id);
    if (elem) {
        elem.quantity = newQuantity;
    }
}

/**
 * edytuje date produktu na podstawie id
 *
 * @param {number} id - identyfikator produktu
 * @param {string} dueDateString - data
 * @returns {void} - funkcja nie zwraca wartości, modyfikuje produkt w miejscu
 */
 function edit_dueDate(id, dueDateString) {
    const elem = productList.find(elem => elem.id === id); 
    if (elem) {
        elem.dueDate = new Date(dueDateString);
    }
}

/**
 * zmienia pozycje produktu na podstawie id
 *
 * @param {number} id - identyfikator produktu
 * @param {number} newPosition - nowa pozycja 
 * @returns {void} - funkcja nie zwraca wartości, modyfikuje produkt w miejscu
 */
function change_position(id, newPosition){
    const index = productList.findIndex(elem => elem.id === id);
    if(index !== -1 && newPosition >= 0 && newPosition < productList.length) {
        const [movedProduct] = productList.splice(index, 1); 
        productList.splice(newPosition, 0, movedProduct);
    }
}

/**
 * zwraca listę produktów, które powinny zostać zakupione dzisiaj
 *
 * @returns {Product[]} - tablica produktów do zakupu na dzisiaj
 */
function purchased_today(){
    const today = new Date().toISOString().split("T")[0];
    return productList.filter(elem => 
        !elem.isPurchased && elem.dueDate.toISOString().split("T")[0] === today
    );
}

/**
 * ustawia cenę za sztukę dla zakupionego produktu
 *
 * @param {number} id - identyfikator produktu
 * @param {number} pricePerUnit - cena za sztukę
 * @returns {void} - funkcja nie zwraca wartości, modyfikuje produkt w miejscu
 */
 function set_price(id, pricePerUnit) {
    const elem = productList.find(elem => elem.id === id);

    if (elem && elem.isPurchased) {
        elem.pricePerUnit = pricePerUnit;
    }
}

/**
 * oblicza koszt zakupionych produktów dla podanej daty
 *
 * @param {string} dateString - data zakupów w formacie "YYYY-MM-DD".
 * @returns {number} - suma kosztów zakupionych produktów (ignoruje produkty bez ceny)
 */
 function calculate_today(dateString) {
    const targetDate = new Date(dateString).toISOString().split("T")[0]; 

    return productList
        .filter(elem => 
            elem.isPurchased && 
            elem.dueDate.toISOString().split("T")[0] === targetDate && 
            typeof elem.pricePerUnit === "number"
        )
        .reduce((total, elem) => total + elem.quantity * elem.pricePerUnit, 0);
}

/**
 * modyfikuje wybrane produkty w liście zakupów
 *
 * @param {number[]} productIds - tablica identyfikatorów produktów do modyfikacji
 * @param {(product: Product) => Product} modifyFunction - funkcja modyfikująca produkt
 * @returns {void} - funkcja nie zwraca wartości, modyfikuje produkty w miejscu
 */
 function modify_all(productIds, modifyFunction) {
    productList = productList.map(elem => 
        productIds.includes(elem.id) ? modifyFunction(elem) : elem
    );
}

// dodawanie produktow
const milk = add_product("Mleko", 2, "2025-03-27");
const bread = add_product("Chleb", 1, "2025-03-27");
const butter = add_product("Masło", 1, "2025-03-28");

console.log("Lista po dodaniu produktów:", productList);

// edycja nazwy produktu
edit_name(milk.id, "Mleko 3.2%");
console.log("Po zmianie nazwy mleka:", productList);

// oznaczenie produktu jako zakupionego
edit_status(butter.id, true);
console.log("Po oznaczeniu masła jako zakupionego:", productList);

// edycja liczby sztuk
edit_quantity(milk.id, 3);
console.log("Po zwiększeniu ilości mleka:", productList);

// edycja daty zakupu 
edit_dueDate(milk.id, "2025-03-30");
console.log("Po zmianie daty zakupu mleka:", productList);

// zmiana pozycji produktu w liście (przeniesienie masła na początek)
change_position(butter.id, 0);
console.log("Po zmianie kolejności:", productList);

// pobranie produktów do zakupu na dziś cus nie tak
console.log("Produkty do zakupu dzisiaj:", purchased_today());

// ustawienie ceny dla zakupionego produktu
set_price(butter.id, 3.50);
console.log("Po ustawieniu ceny masła:", productList);

// obliczenie kosztu zakupów dla danej daty
console.log("Koszt zakupów 27 marca 2025:", calculate_today("2025-03-28"));

// masowe przewalutowanie ceny zakupionych produktów (np. na EUR)
modify_all([butter.id], product => ({ ...product, pricePerUnit: product.pricePerUnit * 0.85 }));
console.log("Po przewalutowaniu ceny:", productList);

// masowe oznaczenie produktów jako zakupione
modify_all([milk.id, butter.id], product => ({ ...product, isPurchased: true }));
console.log("Po masowym oznaczeniu jako zakupione:", productList);



