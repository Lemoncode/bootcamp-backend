import express from 'express';
import cors from 'cors';
import path from 'path';
import { booksApi } from './books.api';

const app = express();
app.use(express.json());
app.use(
  cors({
    methods: 'GET',
    origin: 'http://localhost:8080',
    credentials: true,
  })
);

// TODO: Feed env variable in production
app.use('/', express.static(path.resolve(__dirname, '../public')));

app.use(async (req, res, next) => {
  console.log(req.url);
  next();
});

app.use('/api/books', booksApi);

app.use(async (error, req, res, next) => {
  console.error(error);
  res.sendStatus(500);
});

app.listen(3000, () => {
  console.log('Server ready at port 3000');
});
