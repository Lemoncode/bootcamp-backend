import { GraphQLResolveInfo } from 'graphql';
import { logger } from '#core/logger/index.js';
import { bookRepository } from '#dals/index.js';
import { Book } from '../book.api-model.js';
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
  mapBookFromApiToModel,
} from '../book.mappers.js';

// TODO: Move to common/models/graphql.model.ts
// Pending to add Context type
type GraphQLResolver<Args, Context, ReturnType> = (
  args: Args,
  context: Context,
  info: GraphQLResolveInfo
) => Promise<ReturnType>;

interface BookResolvers {
  books: GraphQLResolver<{ page?: number; pageSize?: number }, unknown, Book[]>;
  book: GraphQLResolver<{ id: string }, unknown, Book>;
  saveBook: GraphQLResolver<{ book: Book }, unknown, Book>;
  deleteBook: GraphQLResolver<{ id: string }, unknown, boolean>;
}

export const bookResolvers: BookResolvers = {
  books: async ({ page, pageSize }) => {
    const bookList = await bookRepository.getBookList(page, pageSize);
    return mapBookListFromModelToApi(bookList);
  },
  book: async ({ id }) => {
    const book = await bookRepository.getBook(id);
    return mapBookFromModelToApi(book);
  },
  saveBook: async ({ book }) => {
    const bookToSave = mapBookFromApiToModel(book);
    const savedBook = await bookRepository.saveBook(bookToSave);
    return mapBookFromModelToApi(savedBook);
  },
  deleteBook: async ({ id }) => {
    const isDeleted = await bookRepository.deleteBook(id);
    if (!isDeleted) {
      const message = `Cannot delete book with id: ${id}`;
      logger.warn(message);
      throw new Error(JSON.stringify({ message, statusCode: 404 }));
    }
    return true;
  },
};
