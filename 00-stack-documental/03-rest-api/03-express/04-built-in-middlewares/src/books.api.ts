import { Router } from "express";
import {
  getBookList,
  getBook,
  insertBook,
  updateBook,
  deleteBook,
} from "./mock-db";

export const booksApi = Router();

booksApi
  .get("/", async (req, res) => {
    const bookList = await getBookList();
    res.send(bookList);
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
