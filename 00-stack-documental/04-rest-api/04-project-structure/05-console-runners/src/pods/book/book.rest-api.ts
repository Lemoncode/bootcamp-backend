import { Router } from "express";
import { bookRepository } from "#dals/index.js";
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
  mapBookFromApiToModel,
} from "./book.mappers.js";

export const booksApi = Router();

booksApi
  .get("/", async (req, res, next) => {
    try {
      const page = Number(req.query.page);
      const pageSize = Number(req.query.pageSize);
      const bookList = await bookRepository.getBookList(page, pageSize);

      res.send(mapBookListFromModelToApi(bookList));
    } catch (error) {
      next(error);
    }
  })
  .get("/:id", async (req, res, next) => {
    try {
      const { id } = req.params;
      const book = await bookRepository.getBook(id);
      res.send(mapBookFromModelToApi(book));
    } catch (error) {
      next(error);
    }
  })
  .post("/", async (req, res, next) => {
    try {
      const book = mapBookFromApiToModel(req.body);
      const newBook = await bookRepository.saveBook(book);
      res.status(201).send(mapBookFromModelToApi(newBook));
    } catch (error) {
      next(error);
    }
  })
  .put("/:id", async (req, res, next) => {
    try {
      const { id } = req.params;
      const book = mapBookFromApiToModel({ ...req.body, id });
      await bookRepository.saveBook(book);
      res.sendStatus(204);
    } catch (error) {
      next(error);
    }
  })
  .delete("/:id", async (req, res, next) => {
    try {
      const { id } = req.params;
      await bookRepository.deleteBook(id);
      res.sendStatus(204);
    } catch (error) {
      next(error);
    }
  });
