import { buildSchema as graphql } from 'graphql';
import { securityDirectives } from '#pods/security/graphql/index.js';

export const bookSchema = securityDirectives(
  graphql(`
    directive @isAuthenticated on FIELD_DEFINITION

    type Book {
      id: String!
      title: String!
      releaseDate: String!
      author: String!
    }

    type Query {
      books(page: Int, pageSize: Int): [Book!]! @isAuthenticated
      book(id: ID!): Book! @isAuthenticated
    }

    input BookInput {
      id: String
      title: String!
      releaseDate: String!
      author: String!
    }

    type Mutation {
      saveBook(book: BookInput!): Book! @isAuthenticated
      deleteBook(id: ID!): Boolean! @isAuthenticated
    }
  `)
);
