import express from 'express';
import path from 'path';
import { GraphQLContext } from 'common-app/models';
import {
  createRestApiServer,
  connectToDBServer,
  createGraphQLServer,
} from 'core/servers';
import { envConstants } from 'core/constants';
import { logger } from 'core/logger';
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from 'common/middlewares';
import { booksApi, bookTypeDefs, bookResolvers } from 'pods/book';
import {
  securityApi,
  authenticationMiddleware,
  securityTypeDefs,
  securityResolvers,
  securityDirectives,
} from 'pods/security';
import { userApi } from 'pods/user';

(async function () {
  const restApiServer = createRestApiServer();
  const graphqlServer = await createGraphQLServer(
    restApiServer,
    {
      typeDefs: [securityTypeDefs, bookTypeDefs],
      resolvers: [securityResolvers, bookResolvers],
      directiveResolvers: securityDirectives,
    },
    {
      context: async ({ req, res }): Promise<GraphQLContext> => ({
        req,
        res,
      }),
    }
  );

  const staticFilesPath = path.resolve(
    __dirname,
    envConstants.STATIC_FILES_PATH
  );
  restApiServer.use('/', express.static(staticFilesPath));

  restApiServer.use(logRequestMiddleware(logger));

  restApiServer.use('/api/security', securityApi);
  restApiServer.use('/api/books', authenticationMiddleware, booksApi);
  restApiServer.use('/api/users', authenticationMiddleware, userApi);

  restApiServer.use(logErrorRequestMiddleware(logger));

  restApiServer.listen(envConstants.PORT, async () => {
    if (!envConstants.isApiMock) {
      await connectToDBServer(envConstants.MONGODB_URI);
      logger.info('Connected to DB');
    } else {
      logger.info('Running API mock');
    }
    logger.info(`Server ready at port ${envConstants.PORT}`);
    logger.info(
      `GraphQL server ready at http://localhost:${envConstants.PORT}${graphqlServer.graphqlPath}`
    );
  });
})();
