import { Express } from 'express';
import { createHandler, HandlerOptions } from 'graphql-http/lib/use/express';
import playground from 'graphql-playground-middleware-express';
const graphqlPlayground = playground.default;
import { envConstants } from '#core/constants/index.js';

export const createGraphqlServer = (
  expressApp: Express,
  options: HandlerOptions
) => {
  expressApp.use('/graphql', createHandler(options));
  if (!envConstants.isProduction) {
    expressApp.use('/playground', graphqlPlayground({ endpoint: '/graphql' }));
  }
};
