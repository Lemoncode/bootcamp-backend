import { gql } from 'apollo-server-express';

export const bookTypeDefs = gql`
  type Book {
    id: String!
    title: String!
    releaseDate: String!
    author: String!
  }

  extend type Query {
    books(page: Int, pageSize: Int): [Book!]! @isAuthenticated
    book(id: ID!): Book! @isAuthenticated
  }

  input BookInput {
    id: String
    title: String!
    releaseDate: String!
    author: String!
  }

  extend type Mutation {
    saveBook(book: BookInput!): Book! @isAuthenticated
    deleteBook(id: ID!): Boolean! @isAuthenticated
  }
`;
