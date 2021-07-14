import { bookRepository } from 'dals';
import { Book } from '../book.api-model';
import { mapBookListFromModelToApi } from '../book.mappers';

export const bookResolvers = {
  Query: {
    books: async (): Promise<Book[]> => {
      const bookList = await bookRepository.getBookList();
      return mapBookListFromModelToApi(bookList);
    },
  },
};
