require('dotenv').config();
const neo4j = require('neo4j-driver');

const uri = process.env.NEO4J_URI;
const user = process.env.NEO4J_USERNAME;
const password = process.env.NEO4J_PASSWORD;


const driver = neo4j.driver(uri, neo4j.auth.basic(user, password));

async function createData() {
    const session = driver.session();

    try {
        const query = `
        CREATE
          (TheMatrix:Movie {title: 'The Matrix', released: 1999, tagline: 'Welcome to the Real World'}),
          (Keanu:Person {name: 'Keanu Reeves', born: 1964}),
          (Carrie:Person {name: 'Carrie-Anne Moss', born: 1967}),
          (Laurence:Person {name: 'Laurence Fishburne', born: 1961}),
          (Hugo:Person {name: 'Hugo Weaving', born: 1960}),
          (LillyW:Person {name: 'Lilly Wachowski', born: 1967}),
          (LanaW:Person {name: 'Lana Wachowski', born: 1965}),
          (JoelS:Person {name: 'Joel Silver', born: 1952}),
          (Keanu)-[:ACTED_IN {roles: ['Neo']}]->(TheMatrix),
          (Carrie)-[:ACTED_IN {roles: ['Trinity']}]->(TheMatrix),
          (Laurence)-[:ACTED_IN {roles: ['Morpheus']}]->(TheMatrix),
          (Hugo)-[:ACTED_IN {roles: ['Agent Smith']}]->(TheMatrix),
          (LillyW)-[:DIRECTED]->(TheMatrix),
          (LanaW)-[:DIRECTED]->(TheMatrix),
          (JoelS)-[:PRODUCED]->(TheMatrix)
        `;
        
        await session.run(query);
        console.log('Data created successfully!');
    } catch (error) {
        console.error('Error creating data:', error);
    } finally {
        await session.close();
        await driver.close();
    }
}

createData();
