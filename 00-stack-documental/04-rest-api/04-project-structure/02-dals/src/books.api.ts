import { Router } from "express";
import { bookRepository } from "#dals/index.js";
import { getBook, insertBook, updateBook, deleteBook } from "./mock-db.js";

export const booksApi = Router();

booksApi
  .get("/", async (req, res, next) => {
    try {
      const page = Number(req.query.page);
      const pageSize = Number(req.query.pageSize);
      let bookList = await bookRepository.getBookList();

      if (page && pageSize) {
        const startIndex = (page - 1) * pageSize;
        const endIndex = Math.min(startIndex + pageSize, bookList.length);
        bookList = bookList.slice(startIndex, endIndex);
      }

      res.send(bookList);
    } catch (error) {
      next(error);
    }
  })
  .get("/:id", async (req, res) => {
    const { id } = req.params;
    const bookId = Number(id);
    const book = await getBook(bookId);
    res.send(book);
  })
  .post("/", async (req, res) => {
    const book = req.body;
    const newBook = await insertBook(book);
    res.status(201).send(newBook);
  })
  .put("/:id", async (req, res) => {
    const { id } = req.params;
    const bookId = Number(id);
    const book = req.body;
    await updateBook(bookId, book);
    res.sendStatus(204);
  })
  .delete("/:id", async (req, res) => {
    const { id } = req.params;
    const bookId = Number(id);
    await deleteBook(bookId);
    res.sendStatus(204);
  });
