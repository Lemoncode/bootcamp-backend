import { gql } from 'apollo-server-express';

// TODO: Implement Void Custom Scalar Type

export const securityTypeDefs = gql`
  directive @isAuthenticated on FIELD_DEFINITION

  extend type Mutation {
    login(email: String!, password: String!): Boolean
    logout: Boolean @isAuthenticated
  }
`;
