import express from 'express';
import path from 'node:path';
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from '#common/middlewares/index.js';
import { createRestApiServer, dbServer } from '#core/servers/index.js';
import { ENV } from '#core/constants/index.js';
import { authenticationMiddleware } from '#core/security/index.js';
import { logger } from '#core/logger/index.js';
import { bookApi } from '#pods/book/index.js';
import { securityApi } from '#pods/security/index.js';
import { userApi } from '#pods/user/index.js';

const app = createRestApiServer();

app.use(
  '/',
  express.static(path.resolve(import.meta.dirname, ENV.STATIC_FILES_PATH))
);

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
});
