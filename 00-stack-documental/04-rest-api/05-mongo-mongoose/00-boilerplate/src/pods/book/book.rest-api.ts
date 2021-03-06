import { Router } from 'express';
import { bookRepository } from 'dals';
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
  mapBookFromApiToModel,
} from './book.mappers';
import { paginateBookList } from './book.helpers';

export const booksApi = Router();

booksApi
  .get('/', async (req, res, next) => {
    try {
      const page = Number(req.query.page);
      const pageSize = Number(req.query.pageSize);
      const bookList = await bookRepository.getBookList();
      const paginatedBookList = paginateBookList(bookList, page, pageSize);
      res.send(mapBookListFromModelToApi(paginatedBookList));
    } catch (error) {
      next(error);
    }
  })
  .get('/:id', async (req, res, next) => {
    try {
      const { id } = req.params;
      const book = await bookRepository.getBook(id);
      res.send(mapBookFromModelToApi(book));
    } catch (error) {
      next(error);
    }
  })
  .post('/', async (req, res, next) => {
    try {
      const modelBook = mapBookFromApiToModel(req.body);
      const newBook = await bookRepository.saveBook(modelBook);
      res.status(201).send(mapBookFromModelToApi(newBook));
    } catch (error) {
      next(error);
    }
  })
  .put('/:id', async (req, res, next) => {
    try {
      const { id } = req.params;
      const modelBook = mapBookFromApiToModel({ ...req.body, id });
      await bookRepository.saveBook(modelBook);
      res.sendStatus(204);
    } catch (error) {
      next(error);
    }
  })
  .delete('/:id', async (req, res, next) => {
    try {
      const { id } = req.params;
      await bookRepository.deleteBook(id);
      res.sendStatus(204);
    } catch (error) {
      next(error);
    }
  });
