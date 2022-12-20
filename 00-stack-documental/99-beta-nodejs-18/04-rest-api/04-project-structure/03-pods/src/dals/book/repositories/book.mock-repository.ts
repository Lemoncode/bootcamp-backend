import { BookRepository } from "./book.repository.js";
import { Book } from "../book.model.js";
import { db } from "../../mock-data.js";

const insertBook = (book: Book) => {
  const id = (db.books.length + 1).toString();
  const newBook: Book = {
    ...book,
    id,
  };

  db.books = [...db.books, newBook];
  return newBook;
};

const updateBook = (book: Book) => {
  db.books = db.books.map((b) => (b.id === book.id ? { ...b, ...book } : b));
  return book;
};

const paginateBookList = (
  bookList: Book[],
  page: number,
  pageSize: number
): Book[] => {
  let paginatedBookList = [...bookList];
  if (page && pageSize) {
    const startIndex = (page - 1) * pageSize;
    const endIndex = Math.min(startIndex + pageSize, paginatedBookList.length);
    paginatedBookList = paginatedBookList.slice(startIndex, endIndex);
  }

  return paginatedBookList;
};

export const mockRepository: BookRepository = {
  getBookList: async (page?: number, pageSize?: number) =>
    paginateBookList(db.books, page, pageSize),
  getBook: async (id: string) => db.books.find((b) => b.id === id),
  saveBook: async (book: Book) =>
    Boolean(book.id) ? updateBook(book) : insertBook(book),
  deleteBook: async (id: string) => {
    db.books = db.books.filter((b) => b.id !== id);
    return true;
  },
};
