import { UserInputError } from 'apollo-server-express';
import { GraphQLResolveInfo } from 'graphql';
import { IResolvers } from '@graphql-tools/utils';
import { logger } from 'core/logger';
import { bookRepository } from 'dals';
import { Book } from '../book.api-model';
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
  mapBookFromApiToModel,
} from '../book.mappers';
import { paginateBookList } from '../book.helpers';

// TODO: Move to common/models/graphql.model.ts
// Add more types when needed
type GraphQLResolver<ReturnedType, Args = any> = (
  rootObject: any,
  args: Args,
  context: any,
  info: GraphQLResolveInfo
) => Promise<ReturnedType>;

interface BookResolvers extends IResolvers {
  Query: {
    books: GraphQLResolver<Book[], { page?: number; pageSize?: number }>;
    book: GraphQLResolver<Book, { id: string }>;
  };
  Mutation: {
    saveBook: GraphQLResolver<Book, { book: Book }>;
    deleteBook: GraphQLResolver<boolean, { id: string }>;
  };
}

export const bookResolvers: BookResolvers = {
  Query: {
    books: async (_, { page, pageSize }) => {
      const bookList = await bookRepository.getBookList();
      const paginatedBookList = paginateBookList(bookList, page, pageSize);
      return mapBookListFromModelToApi(paginatedBookList);
    },
    book: async (_, { id }) => {
      const book = await bookRepository.getBook(id);
      return mapBookFromModelToApi(book);
    },
  },
  Mutation: {
    saveBook: async (_, { book }) => {
      const modelBook = mapBookFromApiToModel(book);
      const newBook = await bookRepository.saveBook(modelBook);
      return mapBookFromModelToApi(newBook);
    },
    deleteBook: async (_, { id }) => {
      const isDeleted = await bookRepository.deleteBook(id);
      if (!isDeleted) {
        const message = `Cannot delete book for id: ${id}`;
        logger.warn(message);
        throw new UserInputError(message);
      }
      return isDeleted;
    },
  },
};
