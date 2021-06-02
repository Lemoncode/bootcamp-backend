import express from 'express';
import { getBookList, getBook } from './mock-db';

const app = express();

app.get('/', (req, res) => {
  res.send('My awesome books portal');
});

app.get('/api/books', async (req, res) => {
  const bookList = await getBookList();
  res.send(bookList);
});

app.get('/api/books/:id', async (req, res) => {
  const { id } = req.params;
  const bookId = Number(id);
  const book = await getBook(bookId);
  res.send(book);
});

app.listen(3000, () => {
  console.log('Server ready at port 3000');
});
