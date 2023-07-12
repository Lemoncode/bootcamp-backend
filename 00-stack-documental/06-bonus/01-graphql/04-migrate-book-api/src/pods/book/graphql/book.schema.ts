import { buildSchema as graphql } from 'graphql';

export const bookSchema = graphql(`
  type Book {
    id: String!
    title: String!
    releaseDate: String!
    author: String!
  }

  type Query {
    books(page: Int, pageSize: Int): [Book!]!
    book(id: ID!): Book!
  }

  input BookInput {
    id: String
    title: String!
    releaseDate: String!
    author: String!
  }

  type Mutation {
    saveBook(book: BookInput!): Book!
    deleteBook(id: ID!): Boolean!
  }
`);
