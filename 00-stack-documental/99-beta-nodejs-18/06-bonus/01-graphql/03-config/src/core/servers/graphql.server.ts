import { ApolloServer, ApolloServerExpressConfig } from 'apollo-server-express';

export const createGraphQLServer = async (
  expressApp,
  config: ApolloServerExpressConfig
) => {
  const graphqlServer = new ApolloServer(config);
  await graphqlServer.start();

  graphqlServer.applyMiddleware({ app: expressApp });
  return graphqlServer;
};
