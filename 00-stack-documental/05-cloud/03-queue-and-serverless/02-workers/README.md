# 02 Workers

In this example we are going to configure two workers for read messages.

We will start from `01-config`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
cd back
npm install

cd consumers
npm install

```

Let's remove previous `dummy queue`:

_./back/src/index.ts_

```diff
import {
  createRestApiServer,
  connectToDBServer,
  connectToMessageBrokerServer,
- messageBroker,
} from '#core/servers/index.js';

...

restApiServer.listen(envConstants.PORT, async () => {
  if (!envConstants.isApiMock) {
    await connectToDBServer(envConstants.MONGODB_URI);
    console.log('Connected to DB');
  } else {
    console.log('Running API mock');
  }
  await connectToMessageBrokerServer(envConstants.RABBITMQ_URL);
- const channel = await messageBroker.channel();
- const queue = await channel.queue('hello-queue', { durable: false });
- await queue.publish('Hello Rabbit!');

  console.log(`Server ready at port ${envConstants.PORT}`);
});

```

Let's create a `book price archive`:

_./back/src/pods/books/book.rest-api.ts_

```diff
import { Router } from 'express';
+ import { messageBroker } from '#core/servers/index.js';
- import { bookRepository } from '#dals/index.js';
+ import { Book, bookRepository } from '#dals/index.js';
import { authorizationMiddleware } from '#pods/security/index.js';
import {
  mapBookListFromModelToApi,
  mapBookFromModelToApi,
  mapBookFromApiToModel,
} from './book.mappers.js';

export const booksApi = Router();

+ const sendBookToArchive = async (book: Book) => {
+   const queueName = 'price-archive-queue';
+   const channel = await messageBroker.channel(1);
+   const queue = await channel.queue(queueName, { durable: true });
+   await queue.publish(JSON.stringify(book), { deliveryMode: 2 });
+ };

...

  .post('/', authorizationMiddleware(['admin']), async (req, res, next) => {
    try {
      const book = mapBookFromApiToModel(req.body);
      const newBook = await bookRepository.saveBook(book);
+     await sendBookToArchive(newBook);
      res.status(201).send(mapBookFromModelToApi(newBook));
    } catch (error) {
      next(error);
    }
  })
  .put('/:id', authorizationMiddleware(['admin']), async (req, res, next) => {
    try {
      const { id } = req.params;
      if (await bookRepository.getBook(id)) {
        const book = mapBookFromApiToModel({ ...req.body, id });
        await bookRepository.saveBook(book);
+       await sendBookToArchive(book);
        res.sendStatus(204);
      } else {
        res.sendStatus(404);
      }
    } catch (error) {
      next(error);
    }
  })
```

> queue `durable`: The server keeps the queue with its own config available after some restart or issue.
>
> deliveryMode 2 -> `persistent` message: If a queue is durable, this message persist after some restart or issue.

Run insert new book:

```bash
cd back
npm start

```

> Open http://localhost:15672
 
Run these urls in the POSTMAN app:

```
URL: http://localhost:3000/api/security/login
METHOD: POST
BODY:
{
	"email": "admin@email.com",
	"password": "test"
}
```

```
URL: http://localhost:3000/api/books
METHOD: POST
BODY:
{
    "title": "My new book",
    "releaseDate": "2022-09-10T00:00:00.000Z",
    "author": "John Doe",
    "price": 20
}
```

> We can send several requests to the queue, we always have the Nodejs thread available.

Create consumers:

_./consumers/src/index.ts_

```diff
import '#core/load-env.js';
+ import { AMQPChannel, QueueParams } from '@cloudamqp/amqp-client';
import { envConstants } from '#core/constants/index.js';
import {
  connectToMessageBrokerServer,
  messageBroker,
} from '#core/servers/index.js';

+ const priceArchiveConsumerOne = async (
+   channel: AMQPChannel,
+   queueName: string,
+   queueParams: QueueParams
+ ) => {
+   try {
+     const queue = await channel.queue(queueName, queueParams);
+     await queue.subscribe(
+       {
+         noAck: false,
+       },
+       (message) => {
+         console.log('Worker 1 message received');
+         const book = JSON.parse(message.bodyToString());
+         console.log(
+           `Saving book with title "${book.title}" and price ${book.price}`
+         );
+         message.ack();
+       }
+     );
+     console.log('Price archive consumer 1 configured');
+   } catch (error) {
+     console.error(error);
+   }
+ };

+ const priceArchiveConsumerTwo = async (
+   channel: AMQPChannel,
+   queueName: string,
+   queueParams: QueueParams
+ ) => {
+   try {
+     const queue = await channel.queue(queueName, queueParams);
+     await queue.subscribe(
+       {
+         noAck: false,
+       },
+       (message) => {
+         console.log('Worker 2 message received');
+         const book = JSON.parse(message.bodyToString());
+         console.log(
+           `Saving book with title "${book.title}" and price ${book.price}`
+         );
+         message.ack();
+       }
+     );
+     console.log('Price archive consumer 2 configured');
+   } catch (error) {
+     console.error(error);
+   }
+ };

- try {
    await connectToMessageBrokerServer(envConstants.RABBITMQ_URL);
+   const channel = await messageBroker.channel(2);
+   const queueName = 'price-archive-queue';
+   const queueParams: QueueParams = { durable: true };
+   await priceArchiveConsumerOne(channel, queueName, queueParams);
+   await priceArchiveConsumerTwo(channel, queueName, queueParams);
-   const channel = await messageBroker.channel();
-   const queue = await channel.queue('hello-queue', { durable: false });
-   await queue.subscribe(
-     {
-       noAck: true,
-     },
-     (message) => {
-       console.log(message.bodyToString());
-     }
-   );
-   console.log('Hello consumer configured');
- } catch (error) {
-   console.error(error);
- }

```

> By default, RabbitMQ will send each message to the next consumer, in sequence. On average every consumer will get the same number of messages. This way of distributing messages is called round-robin.

What happens if we forgot to send the `ack`? 

_./consumers/src/index.ts_

```diff
...
const priceArchiveConsumerOne = async (
  channel: AMQPChannel,
  queueName: string,
  queueParams: QueueParams
) => {
  try {
    const queue = await channel.queue(queueName, queueParams);
    await queue.subscribe(
      {
        noAck: false,
      },
      (message) => {
        console.log('Worker 1 message received');
        const book = JSON.parse(message.bodyToString());
        console.log(
          `Saving book with title "${book.title}" and price ${book.price}`
        );
-       message.ack();
+       // message.ack();
      }
    );
    console.log('Price archive consumer 1 configured');
  } catch (error) {
    console.error(error);
  }
};
...
```

The first worker is getting more messages without resolve the first one, let's solve this using `prefetch`:

_./consumers/src/index.ts_

```diff
...

await connectToMessageBrokerServer(envConstants.RABBITMQ_URL);
const channel = await messageBroker.channel(2);
+ channel.prefetch(1);
const queueName = 'price-archive-queue';
const queueParams: QueueParams = { durable: true };
await priceArchiveConsumerOne(channel, queueName, queueParams);
await priceArchiveConsumerTwo(channel, queueName, queueParams);
...
```

Let's stop and remove the current message broker server to create a new one:

```bash
docker-compose down

```

And add a new volume to persist the message broker data:

_./docker-compose.yml_

```diff
version: '3.8'
services:
  book-store-db:
    container_name: book-store-db
    image: mongo:6
    ports:
      - '27017:27017'
    volumes:
      - type: bind
        source: ./mongo-data
        target: /data/db
  message-broker:
    container_name: message-broker
    image: rabbitmq:3.11-management-alpine
    ports:
      - '5672:5672'
      - '15672:15672'
+   hostname: 'localhost'
+   volumes:
+     - type: bind
+       source: ./message-broker-data
+       target: /var/lib/rabbitmq/mnesia/rabbit@localhost
volumes:
  mongo-data:
+ message-broker-data:

```

Add to the .gitingore:

_./.gitignore_

```diff
node_modules
.env
mongo-data
+ message-broker-data
globalConfig.json
public
dist

```

Run again

```bash
docker-compose up -d

```

> And send new books to the queue

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
