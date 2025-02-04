import '#core/monitoring.js';
import express from 'express';
import path from 'node:path';
import { mergeSchemas } from '@graphql-tools/schema';
import { GraphQLContext } from '#core/models/index.js';
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from '#common/middlewares/index.js';
import {
  createRestApiServer,
  dbServer,
  createGraphqlServer,
} from '#core/servers/index.js';
import { ENV } from '#core/constants/index.js';
import { authenticationMiddleware } from '#core/security/index.js';
import { logger } from '#core/logger/index.js';
import { bookApi, bookSchema, bookResolvers } from '#pods/book/index.js';
import {
  securityApi,
  securitySchema,
  securityResolvers,
} from '#pods/security/index.js';
import { userApi } from '#pods/user/index.js';

const app = createRestApiServer();

app.use(
  '/',
  express.static(path.resolve(import.meta.dirname, ENV.STATIC_FILES_PATH))
);

createGraphqlServer(app, {
  schema: mergeSchemas({ schemas: [securitySchema, bookSchema] }),
  rootValue: { ...securityResolvers, ...bookResolvers },
  context: (req): any => {
    const { res } = req.context;
    return { req, res } as GraphQLContext;
  },
  formatError: (error) => {
    const { message, statusCode } = JSON.parse(error.message);
    return {
      ...error,
      message,
      extensions: {
        statusCode,
      },
    };
  },
});

app.use(logRequestMiddleware(logger));

app.use('/api/security', securityApi);
app.use('/api/books', authenticationMiddleware, bookApi);
app.use('/api/users', authenticationMiddleware, userApi);

app.use(logErrorRequestMiddleware(logger));

app.listen(ENV.PORT, async () => {
  if (!ENV.IS_API_MOCK) {
    await dbServer.connect(ENV.MONGODB_URL);
    logger.info('Running DataBase');
  } else {
    logger.info('Running Mock API');
  }
  logger.info(`Server ready at port ${ENV.PORT}`);
  logger.info(`GraphQL Server ready at port ${ENV.PORT}/graphql`);
});
