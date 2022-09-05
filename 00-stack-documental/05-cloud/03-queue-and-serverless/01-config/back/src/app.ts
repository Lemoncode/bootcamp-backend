import express from 'express';
import path from 'path';
import {
  createRestApiServer,
  connectToDBServer,
  connectToMessageBrokerServer,
  messageBroker,
} from 'core/servers';
import { envConstants } from 'core/constants';
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from 'common/middlewares';
import { booksApi } from 'pods/book';
import { securityApi, authenticationMiddleware } from 'pods/security';
import { userApi } from 'pods/user';

const restApiServer = createRestApiServer();

const staticFilesPath = path.resolve(__dirname, envConstants.STATIC_FILES_PATH);
restApiServer.use('/', express.static(staticFilesPath));

restApiServer.use(logRequestMiddleware);

restApiServer.use('/api/security', securityApi);
restApiServer.use('/api/books', authenticationMiddleware, booksApi);
restApiServer.use('/api/users', authenticationMiddleware, userApi);

restApiServer.use(logErrorRequestMiddleware);

restApiServer.listen(envConstants.PORT, async () => {
  if (!envConstants.isApiMock) {
    await connectToDBServer(envConstants.MONGODB_URI);
    console.log('Connected to DB');
  } else {
    console.log('Running API mock');
  }

  await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
  const channel = await messageBroker.channel();
  const queue = await channel.queue('hello-queue', { durable: false });
  await queue.publish('Hello Rabbit!');

  console.log(`Server ready at port ${envConstants.PORT}`);
});
