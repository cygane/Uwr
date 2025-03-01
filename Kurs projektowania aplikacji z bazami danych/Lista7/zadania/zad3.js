require('dotenv').config();
const neo4j = require('neo4j-driver');

const uri = process.env.NEO4J_URI;
const user = process.env.NEO4J_USERNAME;
const password = process.env.NEO4J_PASSWORD;


const driver = neo4j.driver(uri, neo4j.auth.basic(user, password));

async function runQueries() {
    const session = driver.session();

    try {
        // Query 1: Return the movies where person A acted in
        const personA = 'Keanu Reeves'; 
        const actedInMovies = await session.run(
            `
            MATCH (p:Person {name: $personName})-[:ACTED_IN]->(m:Movie)
            RETURN m.title AS MovieTitle
            `,
            { personName: personA }
        );
        console.log('Movies where', personA, 'acted in:');
        actedInMovies.records.forEach(record => console.log(record.get('MovieTitle')));

        // Query 2: Return the movies where person A was both the actor and the director
        const actedAndDirectedMovies = await session.run(
            `
            MATCH (p:Person {name: $personName})-[:ACTED_IN]->(m:Movie)<-[:DIRECTED]-(p)
            RETURN m.title AS MovieTitle
            `,
            { personName: personA }
        );
        console.log('Movies where', personA, 'was both the actor and the director:');
        actedAndDirectedMovies.records.forEach(record => console.log(record.get('MovieTitle')));

        // Query 3: Return actors who didn’t play in any movie
        const actorsWithoutMovies = await session.run(
            `
            MATCH (p:Person)
            WHERE NOT (p)-[:ACTED_IN]->(:Movie)
            RETURN p.name AS ActorName
            `
        );
        console.log('Actors who didnt play in any movie:');
        actorsWithoutMovies.records.forEach(record => console.log(record.get('ActorName')));

        // Query 4: Return actors who played in more than 2 movies
        const actorsWithMultipleMovies = await session.run(
            `
            MATCH (p:Person)-[:ACTED_IN]->(m:Movie)
            WITH p, COUNT(m) AS movieCount
            WHERE movieCount > 2
            RETURN p.name AS ActorName, movieCount
            `
        );
        console.log('Actors who played in more than 2 movies:');
        actorsWithMultipleMovies.records.forEach(record =>
            console.log(record.get('ActorName'), '-', record.get('movieCount'), 'movies')
        );

        // Query 5: Return movies with the largest number of actors
        const moviesWithMostActors = await session.run(
            `
            MATCH (m:Movie)<-[:ACTED_IN]-(p:Person)
            WITH m, COUNT(p) AS actorCount
            RETURN m.title AS MovieTitle, actorCount
            ORDER BY actorCount DESC
            LIMIT 1
            `
        );
        console.log('Movie(s) with the largest number of actors:');
        moviesWithMostActors.records.forEach(record =>
            console.log(record.get('MovieTitle'), '-', record.get('actorCount'), 'actors')
        );

    } catch (error) {
        console.error('Error running queries:', error);
    } finally {
        await session.close();
        await driver.close();
    }
}

runQueries();
