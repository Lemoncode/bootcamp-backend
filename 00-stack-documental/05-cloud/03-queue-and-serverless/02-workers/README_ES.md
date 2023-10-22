# 02 Workers

En este ejemplo vamos a configurar dos workers para leer mensajes.

Tomamos como punto de partida la muestra `01-config``.

# Steps to build it

Si no lo hubieramos hecho previamente, nos toca hacer un `npm install` para instalar las librerías necesarias (en dos proyectos):

```bash
cd back
npm install

cd consumers
npm install

```

Vamos a eliminar el código que creaba la cola de hola rabbit del ejemplo anterior:

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
  await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
- const channel = await messageBroker.channel();
- const queue = await channel.queue('hello-queue', { durable: false });
- await queue.publish('Hello Rabbit!');

  console.log(`Server ready at port ${envConstants.PORT}`);
});

```

En el endpoint de insertar y editar un libro, vamos a crear un método de ayuda para enviar información de precios de un libro a una cola `book price archive`

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

> El parámetro en la cola `durable`: El servidor mantiene la cola con su propia configuración disponible después de algún reinicio o problema.
>
> El parámetro en el mensaje `deliveryMode`: 1 -> `non-persistent` message, 2 -> `persistent` message: Si una cola es durable, este mensaje persistirá después de algún reinicio o problema.

Vamos a ejecutar la insercíon de un libro:

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

> Podemos enviar varios requests a la cola, siempre tendremos el hilo de Nodejs disponible.

Creamos el consumidor:

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
    await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
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

> Por defecto RabbitMQ enviará cada mensaje al siguiente consumidor, en secuencia. En promedio cada consumidor recibirá el mismo número de mensajes. Esta forma de distribuir mensajes se llama round-robin.

¿Qué pasa si nos olvidamos de enviar el `ack`?

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

El primero worker recibe más mensajes sin resolver el primero, esto lo podemos solucionar usando `prefetch`:

> En RabbitMQ podemos usar el método `prefetch` para configurar el número de mensajes que un consumidor puede recibir sin resolver el primero, de esta forma evitamos una acumulación descontrolada en una canal: imagínate que ese consumidor está saturado o ha pegado un castañazo y está inestable.

_./consumers/src/index.ts_

```diff
...

await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
const channel = await messageBroker.channel(2);
+ channel.prefetch(1);
const queueName = 'price-archive-queue';
const queueParams: QueueParams = { durable: true };
await priceArchiveConsumerOne(channel, queueName, queueParams);
await priceArchiveConsumerTwo(channel, queueName, queueParams);
...
```

Vamos parar y eliminar el servidor de message broker y creamos uno nuevo:

```bash
docker-compose down

```

Añadimos un nuevo volumen para persistir los datos de RabbitMQ:

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

Lo añadimos al `.gitignore`:

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

Y ejecutamos de nuevo

```bash
docker-compose up -d

```

Probamos a enviar nuevos mensajes a la cola, ahora podríamos parar el servidor y levantarlo de nuevo y tanto cola como mensajes estarán disponibles.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
