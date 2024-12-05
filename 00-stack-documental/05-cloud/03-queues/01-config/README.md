# 01 Config

In this example we are going to config a message broker server with [RabbitMQ](https://www.rabbitmq.com/).

We will start from `00-boilerplate`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
cd back
npm install

cd consumers
npm install

```

Update docker-compose with [rabbitmq image](https://hub.docker.com/_/rabbitmq/tags)

_./docker-compose.yml_

```diff
services:
  book-store-db:
    container_name: book-store-db
    image: mongo:7
    ports:
      - '27017:27017'
    volumes:
      - ./mongo-data:/data/db
+ message-broker:
+   container_name: message-broker
+   image: rabbitmq:4-management-alpine
+   ports:
+     - '5672:5672'
+     - '15672:15672'
volumes:
  mongo-data:

```
> `x.y-management-alpine`: RabbitMQ server and Management UI (for development purpose)
>
> `x.y-alpine`: only with RabbitMQ server.

Run both services (mongo and rabbitmq):

```bash
docker compose up -d

```

> Open http://localhost:15672 with guest/guest credentials

We are going to install a recommended library which implements AMQP 0-9-1 protocol, [amqp-client](https://github.com/cloudamqp/amqp-client.js).

```bash
cd back

npm install @cloudamqp/amqp-client --save
```

> It includes d.ts files
>
> [Available RabbitMQ clients](https://www.rabbitmq.com/devtools.html#node-dev)
>
> [Supported protocols](https://www.rabbitmq.com/protocols.html)

Create the connection URI as env variable:

_./back/.env.example_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
IS_API_MOCK=true
MONGODB_URL=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET
AWS_ACCESS_KEY_ID=value
AWS_SECRET_ACCESS_KEY=value
AWS_S3_BUCKET=value
+ RABBITMQ_URL=amqp://guest:guest@localhost:5672

```

_./back/.env_

```diff
NODE_ENV=development
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
IS_API_MOCK=true
MONGODB_URL=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET
AWS_ACCESS_KEY_ID=value
AWS_SECRET_ACCESS_KEY=value
AWS_S3_BUCKET=value
+ RABBITMQ_URL=amqp://guest:guest@localhost:5672

```

Update `env.constants`:

_./back/src/core/constants/env.constants.ts_

```diff
export const ENV = {
  IS_PRODUCTION: process.env.NODE_ENV === 'production',
  PORT: Number(process.env.PORT),
  STATIC_FILES_PATH: process.env.STATIC_FILES_PATH,
  CORS_ORIGIN: process.env.CORS_ORIGIN,
  CORS_METHODS: process.env.CORS_METHODS,
  IS_API_MOCK: process.env.IS_API_MOCK === 'true',
  MONGODB_URL: process.env.MONGODB_URL,
  AUTH_SECRET: process.env.AUTH_SECRET,
  AWS_S3_BUCKET: process.env.AWS_S3_BUCKET,
+ RABBITMQ_URL: process.env.RABBITMQ_URL,
};

```

Configure `RabbitMQ` instance:

_./back/src/core/servers/message-broker.server.ts_

```typescript
import { AMQPClient } from '@cloudamqp/amqp-client';
import { ENV } from '#core/constants/index.js';

export const messageBroker = new AMQPClient(ENV.RABBITMQ_URL);

```

Update barrel file:

_./back/src/core/servers/index.ts_

```diff
export * from './rest-api.server.js';
export * from './db.server.js';
+ export * from './message-broker.server.js';

```

Let's update `app` and connect it:

_./back/src/index.ts_

```diff
...
import {
  createRestApiServer,
  dbServer,
+ messageBroker,
} from '#core/servers/index.js';
import { envConstants } from '#core/constants/index.js';
...

app.listen(ENV.PORT, async () => {
  if (!ENV.IS_API_MOCK) {
    await dbServer.connect(ENV.MONGODB_URL);
    console.log('Running DataBase');
  } else {
    console.log('Running Mock API');
  }
+ await messageBroker.connect();
  console.log(`Server ready at port ${ENV.PORT}`);
});

```

Let's create a `dummy publisher` to check if it's working:

_./back/src/index.ts_

```diff
...

app.listen(ENV.PORT, async () => {
  if (!ENV.IS_API_MOCK) {
    await dbServer.connect(ENV.MONGODB_URL);
    console.log('Running DataBase');
  } else {
    console.log('Running Mock API');
  }
  await messageBroker.connect();
+ const channel = await messageBroker.channel();
+ const queue = await channel.queue('hello-queue', { durable: false });
+ await queue.publish('Hello Rabbit!');

  console.log(`Server ready at port ${ENV.PORT}`);
});

```

> `durable`: the queue will survive a broker restart.

Let's run the app:

```bash
cd back
npm start

```

> Open http://localhost:15672

Apply same configuration in `consumers` project:

```bash
cd consumers

npm install @cloudamqp/amqp-client --save
```

_./consumers/.env.example_

```diff
NODE_ENV=development
+ RABBITMQ_URL=amqp://guest:guest@localhost:5672

```

_./consumers/.env_

```diff
NODE_ENV=development
+ RABBITMQ_URL=amqp://guest:guest@localhost:5672

```

_./consumers/src/core/constants/env.constants.ts_

```diff
export const ENV = {
  IS_PRODUCTION: process.env.NODE_ENV === 'production',
+ RABBITMQ_URL: process.env.RABBITMQ_URL,
};

```

_./consumers/src/core/servers/message-broker.server.ts_

```typescript
import { AMQPClient } from '@cloudamqp/amqp-client';
import { ENV } from '#core/constants/index.js';

export const messageBroker = new AMQPClient(ENV.RABBITMQ_URL);

```

_./consumers/src/core/servers/index.ts_

```typescript
export * from './message-broker.server.js';

```

Let's create a consumer:

_./consumers/src/index.ts_

```diff
+ import { messageBroker } from '#core/servers/index.js';

+ try {
+   await messageBroker.connect();
+   const channel = await messageBroker.channel();
+   const queue = await channel.queue('hello-queue', { durable: false });

+   await queue.subscribe(
+     {
+       noAck: true,
+     },
+     (message) => {
+       console.log(message.bodyToString());
+     }
+   );
+   console.log('Hello consumer configured');
+ } catch (error) {
+   console.error(error);
+ }

```

> We have to define the `queue` with the same configuration `name` and `durable`.
>
> This configuration with noAck could be usefull in a sensor app

Run consumers:

```bash
cd consumers
npm start

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
