// Create a new express application

const express = require('express');

const app = express();

app.get('/', (req, res) => {
    res.send('Hello world');
});

app.listen(3000, () => {
    console.log(`Up & running 🚀 at http://localhost:3000`)
});
