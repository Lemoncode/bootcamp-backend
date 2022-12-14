import {
  ApolloServer,
  ApolloServerExpressConfig,
  gql,
} from 'apollo-server-express';
import { DocumentNode } from 'graphql';
import {
  makeExecutableSchema,
  IExecutableSchemaDefinition,
} from '@graphql-tools/schema';
import {
  ApolloServerPluginLandingPageGraphQLPlayground,
  ApolloServerPluginLandingPageDisabled,
} from 'apollo-server-core';
import { envConstants } from 'core/constants';

// NOTE: We cannot defined empty type.
const coreTypeDefs = gql`
  type Query {
    _empty: String
  }

  type Mutation {
    _empty: String
  }
`;

export const createGraphQLServer = async (
  expressApp,
  schemaOptions: IExecutableSchemaDefinition,
  config: ApolloServerExpressConfig,
) => {
  const typeDefs = Array.isArray(schemaOptions.typeDefs)
    ? (schemaOptions.typeDefs as DocumentNode[])
    : ([schemaOptions.typeDefs] as DocumentNode[]);

  const graphqlServer = new ApolloServer({
    ...config,
    schema: makeExecutableSchema({
      ...schemaOptions,
      typeDefs: [coreTypeDefs, ...typeDefs],
    }),
    plugins: [
      envConstants.isProduction
        ? ApolloServerPluginLandingPageDisabled()
        : ApolloServerPluginLandingPageGraphQLPlayground(),
    ],
  });
  await graphqlServer.start();

  graphqlServer.applyMiddleware({ app: expressApp });
  return graphqlServer;
};
