require('dotenv').config();

const neo4j = require('neo4j-driver');

const uri = process.env.NEO4J_URI;
const user = process.env.NEO4J_USERNAME;
const password = process.env.NEO4J_PASSWORD;


const driver = neo4j.driver(uri, neo4j.auth.basic(user, password));

async function runQuery() {
    const session = driver.session();

    try {
        // Query 1: Add new actors and movies
        await session.run(`
            CREATE
              (Tom:Person {name: 'Tom Hardy', born: 1977}),
              (Emily:Person {name: 'Emily Blunt', born: 1983}),
              (Inception:Movie {title: 'Inception', released: 2010, tagline: 'Your mind is the scene of the crime'}),
              (AQuietPlace:Movie {title: 'A Quiet Place', released: 2018, tagline: 'If they hear you, they hunt you'})
        `);

        console.log("Added new actors and movies!");

        // Query 2: Add properties to a movie
        await session.run(`
            MATCH (m:Movie {title: 'The Matrix'})
            SET m.genre = 'Sci-Fi', m.boxOffice = 463517383
        `);

        console.log("Updated movie properties!");

        // Query 3: Add new relationships (split into separate calls)
        await session.run(`
            MATCH (m:Movie {title: 'Inception'}), (a:Person {name: 'Tom Hardy'})
            CREATE (a)-[:ACTED_IN {roles: ['Eames']}]->(m)
        `);

        await session.run(`
            MATCH (m:Movie {title: 'A Quiet Place'}), (a:Person {name: 'Emily Blunt'})
            CREATE (a)-[:ACTED_IN {roles: ['Evelyn Abbott']}]->(m)
        `);

        console.log("Added new relationships!");

        // Query 4: Update a movie property
        await session.run(`
            MATCH (m:Movie {title: 'The Matrix'})
            SET m.released = 1998
        `);

        console.log("Updated a movie property!");

        // Query 5: Remove a relationship
        await session.run(`
            MATCH (p:Person {name: 'Hugo Weaving'})-[r:ACTED_IN]->(m:Movie {title: 'The Matrix'})
            DELETE r
        `);

        console.log("Removed a relationship!");
    } catch (error) {
        console.error("Error running query:", error);
    } finally {
        await session.close();
    }

    await driver.close();
}
