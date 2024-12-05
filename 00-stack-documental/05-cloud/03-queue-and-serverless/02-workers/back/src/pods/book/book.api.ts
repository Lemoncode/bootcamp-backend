import { Router } from 'express';
import { messageBroker } from '#core/servers/index.js';
import { Book, bookRepository } from '#dals/index.js';
import { authorizationMiddleware } from '#core/security/index.js';
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
  mapBookFromApiToModel,
} from './book.mappers.js';

export const bookApi = Router();

const sendBookToArchive = async (book: Book) => {
  const queueName = 'price-archive-queue';
  const channel = await messageBroker.channel(1);
  const queue = await channel.queue(queueName, { durable: true });
  await queue.publish(JSON.stringify(book), { deliveryMode: 2 });
};

bookApi
  .get('/', async (req, res, next) => {
    try {
      const page = Number(req.query.page);
      const pageSize = Number(req.query.pageSize);
      const bookList = await bookRepository.getBookList(page, pageSize);
      res.send(mapBookListFromModelToApi(bookList));
    } catch (error) {
      next(error);
    }
  })
  .get('/:id', async (req, res, next) => {
    try {
      const { id } = req.params;
      const book = await bookRepository.getBook(id);
      if (book) {
        res.send(mapBookFromModelToApi(book));
      } else {
        res.sendStatus(404);
      }
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
