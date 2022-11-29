import { BookRepository } from './book.repository';
import { Book } from '../book.model';

export const dbRepository: BookRepository = {
  getBookList: async () => {
    throw new Error('Not implemented');
  },
  getBook: async (id: string) => {
    throw new Error('Not implemented');
  },
  saveBook: async (book: Book) => {
    throw new Error('Not implemented');
  },
  deleteBook: async (id: string) => {
    throw new Error('Not implemented');
  },
};
