# 01 Config

En este ejemplo vamos a configurar un servidor de mensajería (`message broker server`).

Tomaremos como punto de partida el ejemplo _00-boilerplate_.

# Pasos

Ejecutamos `npm install` para instalar las librerías necesarias (en dos proyectos):

```bash
cd back
npm install

cd consumers
npm install

```

Actualizamos el fichero `docker-compose` con la [imagen de rabbitmq](https://hub.docker.com/_/rabbitmq/tags)

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
+ message-broker:
+   container_name: message-broker
+   image: rabbitmq:3.12-management-alpine
+   ports:
+     - '5672:5672'
+     - '15672:15672'
volumes:
  mongo-data:

```

> `x.y-management-alpine`: RabbitMQ server y Management UI (para desarrollo)
>
> `x.y-alpine`: está sólo tiene el RabbitMQ server.

Y levantamos ambos servicios (mongo y rabbitmq):

```bash
docker-compose up -d
```

> Abrir desde el navegador web: http://localhost:15672 con credenciales guest/guest

Vamos a instalar la librería oficial recomendada que implementa el protocolo AMQP 0-9-1, [amqp-client](https://github.com/cloudamqp/amqp-client.js).

```bash
cd back

npm install @cloudamqp/amqp-client --save
```

> Tienes ues definition tipos (d.ts) includios
>
> [Available RabbitMQ clients](https://www.rabbitmq.com/devtools.html#node-dev)
>
> [Supported protocols](https://www.rabbitmq.com/protocols.html)

Vamos a crear URI de conexión como variable de entorno:

_./back/.env.example_

```diff
NODE_ENV=development
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

Actualizamos `env.constants`:

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

Configuramos la instancia de `RabbitMQ`:

_./back/src/core/servers/message-broker.server.ts_

```typescript
import { AMQPClient } from "@cloudamqp/amqp-client";
import { AMQPBaseClient } from "@cloudamqp/amqp-client/types/amqp-base-client";

export let messageBroker: AMQPBaseClient;

export const connectToMessageBrokerServer = async (connectionURI: string) => {
  const client = new AMQPClient(connectionURI);
  messageBroker = await client.connect();
};
```

> [Related Github issue](https://github.com/cloudamqp/amqp-client.js/issues/63)

Con la configuración anterior de TS, no puede resolver el tipo `AMQPBaseClient`, necesitamos añadir lo siguiente a `tsconfig.json`:

_./back/tsconfig.json_

```diff
{
  "compilerOptions": {
    ...
    "baseUrl": "./src",
    "paths": {
      "#*": ["*"],
+     "@cloudamqp/amqp-client/types/amqp-base-client": [
+       "../node_modules/@cloudamqp/amqp-client/types/amqp-base-client.d.ts"
+     ]
    }
...
```

Actualizamos el barrel file (index):

_./back/src/core/servers/index.ts_

```diff
export * from './rest-api.server.js';
export * from './db.server.js';
+ export * from './message-broker.server.js';

```

Vamos a actualizar `app` y conectarlo:

_./back/src/index.ts_

```diff
...
import {
  createRestApiServer,
  connectToDBServer,
+ connectToMessageBrokerServer,
} from '#core/servers/index.js';
import { envConstants } from '#core/constants/index.js';
...

restApiServer.listen(envConstants.PORT, async () => {
  if (!envConstants.isApiMock) {
    await connectToDBServer(envConstants.MONGODB_URI);
    console.log('Connected to DB');
  } else {
    console.log('Running API mock');
  }
+ await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
  console.log(`Server ready at port ${envConstants.PORT}`);
});

```

Vamos a crear un `dummy publisher` para comprobar si funciona:

_./back/src/index.ts_

```diff
...
import {
  createRestApiServer,
  connectToDBServer,
  connectToMessageBrokerServer,
+ messageBroker,
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
+ const channel = await messageBroker.channel();
+ const queue = await channel.queue('hello-queue', { durable: false });
+ await queue.publish('Hello Rabbit!');

  console.log(`Server ready at port ${envConstants.PORT}`);
});

```

> `durable`: la cola sobrevivirá o no a un reinicio del broker, ¡OJO! Estamos hablando a nivel de cola completa, no de mensaje que se guarden en la cola (estos tenemos que especificarles que sean persistentes).

Ejecutamos el back:

```bash
cd back
npm start

```

> Open http://localhost:15672

Ahora aplicamos la misma configuración en el proyecto `consumers`:

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

Actualizamos el `tsconfig.json`:

_./consumers/tsconfig.json_

```diff
{
  "compilerOptions": {
    ...
    "baseUrl": "./src",
    "paths": {
      "#*": ["*"],
+     "@cloudamqp/amqp-client/types/amqp-base-client": [
+       "../node_modules/@cloudamqp/amqp-client/types/amqp-base-client.d.ts"
+     ]
    }
...
```

_./consumers/src/core/servers/message-broker.server.ts_

```typescript
import { AMQPClient } from "@cloudamqp/amqp-client";
import { AMQPBaseClient } from "@cloudamqp/amqp-client/types/amqp-base-client";

export let messageBroker: AMQPBaseClient;

export const connectToMessageBrokerServer = async (connectionURI: string) => {
  const client = new AMQPClient(connectionURI);
  messageBroker = await client.connect();
};
```

_./consumers/src/core/servers/index.ts_

```typescript
export * from "./message-broker.server.js";
```

Vamos a crear el consumer:

_./consumers/src/index.ts_

```diff
import '#core/load-env.js';
+ import { envConstants } from '#core/constants/index.js';
+ import {
+   connectToMessageBrokerServer,
+   messageBroker,
+ } from '#core/servers/index.js';

+ try {
+   await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
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

> Definimos la cola con la misma configuración de `name` y `durable`.
>
> Esta configuración con `noAck` podría ser útil en una app de sensores.

Ejecutamos el consumer:

```bash
cd consumers
npm start
```

Y vemos como llega el mensaje

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
