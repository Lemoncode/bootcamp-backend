import '#core/load-env.js';
import express from 'express';
import path from 'path';
import url from 'url';
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from '#common/middlewares/index.js';
import { createRestApiServer, connectToDBServer } from '#core/servers/index.js';
import { envConstants } from '#core/constants/index.js';
import { logger } from '#core/logger/index.js';
import { booksApi } from '#pods/book/index.js';
import { securityApi, authenticationMiddleware } from '#pods/security/index.js';
import { userApi } from '#pods/user/index.js';

const restApiServer = createRestApiServer();

const __dirname = path.dirname(url.fileURLToPath(import.meta.url));
const staticFilesPath = path.resolve(__dirname, envConstants.STATIC_FILES_PATH);
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
});
