import { Router } from 'express';
import {
  getBookList,
  getBook,
  insertBook,
  updateBook,
  deleteBook,
} from './mock-db';

export const booksApi = Router();

booksApi
  .get('/', async (req, res, next) => {
    try {
      const bookList = await getBookList();
      throw Error('Simulating error');
      res.send(bookList);
    } catch (error) {
      next(error);
    }
  })
  .get('/:id', async (req, res) => {
    const { id } = req.params;
    const bookId = Number(id);
    const book = await getBook(bookId);
    res.setHeader('Authorization', 'Basic my-user:my-password');
    res.send(book);
  })
  .post('/', async (req, res) => {
    const book = req.body;
    const newBook = await insertBook(book);
    res.status(201).send(newBook);
  })
  .put('/:id', async (req, res) => {
    const { id } = req.params;
    const bookId = Number(id);
    const book = req.body;
    await updateBook(bookId, book);
    res.sendStatus(204);
  })
  .delete('/:id', async (req, res) => {
    const { id } = req.params;
    const bookId = Number(id);
    await deleteBook(bookId);
    res.sendStatus(204);
  });
