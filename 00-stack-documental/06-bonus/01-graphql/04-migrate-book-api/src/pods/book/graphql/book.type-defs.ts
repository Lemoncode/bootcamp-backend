import { gql } from 'apollo-server-express';

export const bookTypeDefs = gql`
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
`;
