require('dotenv').config();
const neo4j = require('neo4j-driver');

const uri = process.env.NEO4J_URI;
const user = process.env.NEO4J_USERNAME;
const password = process.env.NEO4J_PASSWORD;

const driver = neo4j.driver(uri, neo4j.auth.basic(user, password));

async function fetchPersons() {
    const session = driver.session();

    try {
        const result = await session.run(`
            MATCH (p:Person)
            RETURN p.name AS Name, p.born AS Born
        `);

        const persons = result.records.map(record => ({
            Name: record.get('Name'),
            Born: record.get('Born') || 'Unknown', 
        }));

        console.log('Persons in the database:');
        console.log('------------------------------------');
        console.log(`| ${'Name'.padEnd(20)} | ${'Born'.padEnd(10)} |`);
        console.log('------------------------------------');
        persons.forEach(person => {
            console.log(`| ${person.Name.padEnd(20)} | ${String(person.Born).padEnd(10)} |`);
        });
        console.log('------------------------------------');
    } catch (error) {
        console.error('Error fetching persons:', error);
    } finally {
        await session.close();
        await driver.close();
    }
}

fetchPersons();
