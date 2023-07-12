import { bookRepository } from '#dals/index.js';
import { Book } from '../book.api-model.js';
import { mapBookListFromModelToApi } from '../book.mappers.js';

export const bookResolvers = {
  books: async (): Promise<Book[]> => {
    const bookList = await bookRepository.getBookList();
    return mapBookListFromModelToApi(bookList);
  },
};
