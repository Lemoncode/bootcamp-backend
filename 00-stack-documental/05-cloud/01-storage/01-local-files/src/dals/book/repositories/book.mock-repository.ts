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

export const mockRepository: BookRepository = {
  getBookList: async () => db.books,
  getBook: async (id: string) =>
    db.books.find((b) => b._id.toHexString() === id),
  saveBook: async (book: Book) =>
    db.books.some((b) => b._id.toHexString() === book._id.toHexString())
      ? updateBook(book)
      : insertBook(book),
  deleteBook: async (id: string) => {
    db.books = db.books.filter((b) => b._id.toHexString() !== id);
    return true;
  },
};
