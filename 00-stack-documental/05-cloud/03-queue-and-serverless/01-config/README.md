# 01

In this example we are going to config .

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
version: '3.8'
services:
  book-store-db:
    container_name: book-store-db
    image: mongo:5.0.9
    ports:
      - '27017:27017'
    volumes:
      - type: bind
        source: ./mongo-data
        target: /data/db
+ message-broker:
+   container_name: message-broker
+   image: rabbitmq:3.10-management-alpine
+   ports:
+     - '5672:5672'
+     - '15672:15672'
volumes:
  mongo-data:

```

> `3.10-management-alpine`: RabbitMQ server and Management UI (for development purpose)
>
> `3.10-alpine`: only with RabbitMQ server.

Run both services (mongo and rabbitmq):

```bash
docker-compose up -d

```

We are going to install a official recommended library which implements AMQP 0-9-1 protocol, [amqp-client](https://github.com/cloudamqp/amqp-client.js).

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

PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
API_MOCK=true
MONGODB_URI=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET
AWS_ACCESS_KEY_ID=value
AWS_SECRET_ACCESS_KEY=value
AWS_S3_BUCKET=value
+ RABBITMQ_URI=amqp://guest:guest@localhost:5672

```

_./back/.env_

```diff
PORT=3000
STATIC_FILES_PATH=../public
CORS_ORIGIN=*
CORS_METHODS=GET,POST,PUT,DELETE
API_MOCK=true
MONGODB_URI=mongodb://localhost:27017/book-store
AUTH_SECRET=MY_AUTH_SECRET
AWS_ACCESS_KEY_ID=value
AWS_SECRET_ACCESS_KEY=value
AWS_S3_BUCKET=value
+ RABBITMQ_URI=amqp://guest:guest@localhost:5672

```

Update `env.constants`:

_./back/src/core/constants/env.constants.ts_

```diff
export const envConstants = {
  isProduction: process.env.NODE_ENV === 'production',
  PORT: process.env.PORT,
  STATIC_FILES_PATH: process.env.STATIC_FILES_PATH,
  CORS_ORIGIN: process.env.CORS_ORIGIN,
  CORS_METHODS: process.env.CORS_METHODS,
  isApiMock: process.env.API_MOCK === 'true',
  MONGODB_URI: process.env.MONGODB_URI,
  AUTH_SECRET: process.env.AUTH_SECRET,
  AWS_S3_BUCKET: process.env.AWS_S3_BUCKET,
+ RABBITMQ_URI: process.env.RABBITMQ_URI,
};

```

Configure `RabbitMQ` instance:

_./back/src/core/servers/message-broker.server.ts_

```typescript
import { AMQPClient } from '@cloudamqp/amqp-client';
import { AMQPBaseClient } from '@cloudamqp/amqp-client/types/amqp-base-client';

export let messageBroker: AMQPBaseClient;

export const connectToMessageBrokerServer = async (connectionURI: string) => {
  const client = new AMQPClient(connectionURI);
  messageBroker = await client.connect();
};

```

Update barrel file:

_./back/src/core/servers/index.ts_

```diff
export * from './rest-api.server';
export * from './db.server';
+ export * from './message-broker.server';

```

Let's update `app` and connect it:

_./back/src/app.ts_

```diff
import express from 'express';
import path from 'path';
import {
  createRestApiServer,
  connectToDBServer,
+ connectToMessageBrokerServer,
} from 'core/servers';
import { envConstants } from 'core/constants';
...

restApiServer.listen(envConstants.PORT, async () => {
  if (!envConstants.isApiMock) {
    await connectToDBServer(envConstants.MONGODB_URI);
    console.log('Connected to DB');
  } else {
    console.log('Running API mock');
  }

+ await connectToMessageBrokerServer(envConstants.RABBITMQ_URI)

  console.log(`Server ready at port ${envConstants.PORT}`);
});

```

Let's create a `dummy publisher` to check if it's working:L

_./back/src/app.ts_

```diff
import express from 'express';
import path from 'path';
import {
  createRestApiServer,
  connectToDBServer,
  connectToMessageBrokerServer,
+ messageBroker,
} from 'core/servers';

...

restApiServer.listen(envConstants.PORT, async () => {
  if (!envConstants.isApiMock) {
    await connectToDBServer(envConstants.MONGODB_URI);
    console.log('Connected to DB');
  } else {
    console.log('Running API mock');
  }

  await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
+ const channel = await messageBroker.channel();
+ const queue = await channel.queue('hello-queue', { durable: false });
+ await queue.publish('Hello Rabbit!');
    
  console.log(`Server ready at port ${envConstants.PORT}`);
});

```

Apply same configuration in `consumers` project:

```bash
cd consumers

npm install @cloudamqp/amqp-client --save
```

_./consumers/.env.example_

```diff
NODE_ENV=development
+ RABBITMQ_URI=amqp://guest:guest@localhost:5672

```

_./consumers/.env_

```diff
NODE_ENV=development
+ RABBITMQ_URI=amqp://guest:guest@localhost:5672

```

_./consumers/src/core/constants/env.constants.ts_

```diff
export const envConstants = {
  isProduction: process.env.NODE_ENV === 'production',
+ RABBITMQ_URI: process.env.RABBITMQ_URI,
};

```

_./consumers/src/core/servers/message-broker.server.ts_

```typescript
import { AMQPClient } from '@cloudamqp/amqp-client';
import { AMQPBaseClient } from '@cloudamqp/amqp-client/types/amqp-base-client';

export let messageBroker: AMQPBaseClient;

export const connectToMessageBrokerServer = async (connectionURI: string) => {
  const client = new AMQPClient(connectionURI);
  messageBroker = await client.connect();
};

```

_./consumers/src/core/servers/index.ts_

```diff
+ export * from './message-broker.server';

```

Let's create a consumer:

_./consumers/src/app.ts_

```typescript
import { envConstants } from 'core/constants';
import { connectToMessageBrokerServer, messageBroker } from 'core/servers';

const helloConsumer = async () => {
  try {
    await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
    const channel = await messageBroker.channel();
    const queue = await channel.queue('hello-queue', { durable: false });
    await queue.subscribe(
      {
        noAck: true,
      },
      (message) => {
        console.log(message.bodyToString());
      }
    );
    console.log('Hello consumer configured');
  } catch (error) {
    console.error(error);
  }
};

helloConsumer();

```

> We have to define the `queue` with the same configuration `name` and `durable`.
>
> This configuration with noAck could be usefull with a sensor app

Run consumers:

```bash
cd consumers
npm start

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
