import { buildSchema as graphql } from 'graphql';

export const bookSchema = graphql(`
  type Book {
    id: String!
    title: String!
    releaseDate: String!
    author: String!
  }

  type Query {
    books: [Book!]!
  }
`);
