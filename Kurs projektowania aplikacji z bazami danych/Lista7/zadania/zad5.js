require('dotenv').config();
const neo4j = require('neo4j-driver');

const uri = process.env.NEO4J_URI;
const user = process.env.NEO4J_USERNAME;
const password = process.env.NEO4J_PASSWORD;


const driver = neo4j.driver(uri, neo4j.auth.basic(user, password));

class Person {
    constructor(name, born) {
        this.name = name;
        this.born = born;
    }
}

class PersonRepository {
    constructor(driver) {
        this.driver = driver;
    }

    async createPerson(person) {
        const session = this.driver.session();
        try {
            await session.run(
                `
                CREATE (p:Person {name: $name, born: $born})
                RETURN p
                `,
                { name: person.name, born: person.born }
            );
            console.log(`Created person: ${person.name}`);
        } finally {
            await session.close();
        }
    }

    async getAllPersons() {
        const session = this.driver.session();
        try {
            const result = await session.run(`
                MATCH (p:Person)
                RETURN p.name AS name, p.born AS born
            `);

            return result.records.map(record => new Person(
                record.get('name'),
                record.get('born')
            ));
        } finally {
            await session.close();
        }
    }

    async updatePerson(name, updatedData) {
        const session = this.driver.session();
        try {
            await session.run(
                `
                MATCH (p:Person {name: $name})
                SET p += $updatedData
                RETURN p
                `,
                { name, updatedData }
            );
            console.log(`Updated person: ${name}`);
        } finally {
            await session.close();
        }
    }

    async deletePerson(name) {
        const session = this.driver.session();
        try {
            await session.run(
                `
                MATCH (p:Person {name: $name})
                DELETE p
                `,
                { name }
            );
            console.log(`Deleted person: ${name}`);
        } finally {
            await session.close();
        }
    }
}

(async () => {
    const personRepo = new PersonRepository(driver);

    const newPerson = new Person('John Doe', 1985);
    await personRepo.createPerson(newPerson);

    const persons = await personRepo.getAllPersons();
    console.log('All Persons:');
    persons.forEach(person => console.log(`${person.name}, born ${person.born}`));

    await personRepo.updatePerson('John Doe', { born: 1990 });

    const persons2 = await personRepo.getAllPersons();
    console.log('All Persons:');
    persons2.forEach(person => console.log(`${person.name}, born ${person.born}`));

    await personRepo.deletePerson('John Doe');

    await driver.close();
})();
