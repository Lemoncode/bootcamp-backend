import express from 'express';
import path from 'node:path';
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from '#common/middlewares/index.js';
import {
  createRestApiServer,
  dbServer,
  messageBroker,
} from '#core/servers/index.js';
import { ENV } from '#core/constants/index.js';
import { authenticationMiddleware } from '#core/security/index.js';
import { bookApi } from '#pods/book/index.js';
import { securityApi } from '#pods/security/index.js';
import { userApi } from '#pods/user/index.js';

const app = createRestApiServer();

app.use(
  '/',
  express.static(path.resolve(import.meta.dirname, ENV.STATIC_FILES_PATH))
);

app.use(logRequestMiddleware);

app.use('/api/security', securityApi);
app.use('/api/books', authenticationMiddleware, bookApi);
app.use('/api/users', authenticationMiddleware, userApi);

app.use(logErrorRequestMiddleware);

app.listen(ENV.PORT, async () => {
  if (!ENV.IS_API_MOCK) {
    await dbServer.connect(ENV.MONGODB_URL);
    console.log('Running DataBase');
  } else {
    console.log('Running Mock API');
  }
  await messageBroker.connect();
  const channel = await messageBroker.channel();
  const queue = await channel.queue('hello-queue', { durable: false });
  await queue.publish('Hello Rabbit!');

  console.log(`Server ready at port ${ENV.PORT}`);
});
