import { ObjectId } from 'mongodb';
import { BookRepository } from './book.repository';
import { Book } from '../book.model';
import { db } from '../../mock-data';

const insertBook = (book: Book) => {
  const _id = new ObjectId();
  const newBook: Book = {
    ...book,
    _id,
  };

  db.books = [...db.books, newBook];
  return newBook;
};

const updateBook = (book: Book) => {
  db.books = db.books.map((b) =>
    b._id.toHexString() === book._id.toHexString() ? { ...b, ...book } : b
  );
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
  getBook: async (id: string) =>
    db.books.find((b) => b._id.toHexString() === id),
  saveBook: async (book: Book) =>
    Boolean(book._id) ? updateBook(book) : insertBook(book),
  deleteBook: async (id: string) => {
    db.books = db.books.filter((b) => b._id.toHexString() !== id);
    return true;
  },
};
