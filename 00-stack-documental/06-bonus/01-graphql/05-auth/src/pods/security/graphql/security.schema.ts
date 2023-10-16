import { buildSchema as graphql } from 'graphql';
import { securityDirectives } from './security.directives.js';

// TODO: Implement Void Custom Scalar Type

export const securitySchema = securityDirectives(
  graphql(`
    directive @isAuthenticated on FIELD_DEFINITION

    type Mutation {
      login(email: String!, password: String!): Boolean
      logout: Boolean @isAuthenticated
    }
  `)
);
