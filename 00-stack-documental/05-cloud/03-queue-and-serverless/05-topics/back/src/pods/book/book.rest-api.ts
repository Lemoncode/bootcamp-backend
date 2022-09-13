import { Router } from 'express';
import { messageBroker } from 'core/servers';
import { Book, bookRepository } from 'dals';
import { authorizationMiddleware } from 'pods/security';
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
  mapBookFromApiToModel,
} from './book.mappers';

export const booksApi = Router();

const sendBookToArchive = async (book: Book) => {
  const exchangeName = 'price-archive';
  const priceKey = book.price <= 100 ? 'low-prices' : 'high-prices';
  const dateKey =
    book.releaseDate.getFullYear() < new Date().getFullYear() ? 'old' : 'new';
  const routingKey = `${priceKey}.${book.author}.${dateKey}`;
  const channel = await messageBroker.channel(1);
  await channel.basicPublish(exchangeName, routingKey, JSON.stringify(book), {
    deliveryMode: 2,
  });
};

booksApi
  .get('/', authorizationMiddleware(), async (req, res, next) => {
    try {
      const page = Number(req.query.page);
      const pageSize = Number(req.query.pageSize);
      const bookList = await bookRepository.getBookList(page, pageSize);
      res.send(mapBookListFromModelToApi(bookList));
    } catch (error) {
      next(error);
    }
  })
  .get('/:id', authorizationMiddleware(), async (req, res, next) => {
    try {
      const { id } = req.params;
      const book = await bookRepository.getBook(id);
      res.send(mapBookFromModelToApi(book));
    } catch (error) {
      next(error);
    }
  })
  .post('/', authorizationMiddleware(['admin']), async (req, res, next) => {
    try {
      const book = mapBookFromApiToModel(req.body);
      const newBook = await bookRepository.saveBook(book);
      await sendBookToArchive(newBook);
      res.status(201).send(mapBookFromModelToApi(newBook));
    } catch (error) {
      next(error);
    }
  })
  .put('/:id', authorizationMiddleware(['admin']), async (req, res, next) => {
    try {
      const { id } = req.params;
      if (await bookRepository.getBook(id)) {
        const book = mapBookFromApiToModel({ ...req.body, id });
        await bookRepository.saveBook(book);
        await sendBookToArchive(book);
        res.sendStatus(204);
      } else {
        res.sendStatus(404);
      }
    } catch (error) {
      next(error);
    }
  })
  .delete(
    '/:id',
    authorizationMiddleware(['admin']),
    async (req, res, next) => {
      try {
        const { id } = req.params;
        const isDeleted = await bookRepository.deleteBook(id);
        res.sendStatus(isDeleted ? 204 : 404);
      } catch (error) {
        next(error);
      }
    }
  );
