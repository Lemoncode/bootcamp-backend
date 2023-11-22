# 03 Routing

En este ejemplo vamos a configurar el broker de mensajes como un router.

Tomamos como punto de partida el ejemplo `02-workers`.

# Steps to build it

Si no lo hemos hecho antes, vamos a instalar las dependencias en ambos proyectos:

```bash
cd back
npm install

cd consumers
npm install
```

El ejemplo anterior estaba usando el `exchange` por defecto de AMQP, vamos a actualizar el ejemplo pero usando uno explícito:

Esta vez vamos a enruta por el precio del libro.

_./back/src/pods/book/book.rest-api.ts_

```diff
...

const sendBookToArchive = async (book: Book) => {
- const queueName = 'price-archive-queue';
+ const exchangeName = 'price-archive';
+ const routingKey = 'price-key';
  const channel = await messageBroker.channel(1);
- const queue = await channel.queue(queueName, { durable: true });
- await queue.publish(JSON.stringify(book), { deliveryMode: 2 });
+ await channel.exchangeDeclare(exchangeName, 'direct', { durable: true });
+ await channel.basicPublish(exchangeName, routingKey, JSON.stringify(book), {
+   deliveryMode: 2,
+ });
};

...

```

Vamos a actualizar los consumidores para que se suscriban a un `exchange` y no a una `queue`:

_./consumers/src/index.ts_

```diff
...

+ const exchangeName = 'price-archive';
+ const routingKey = 'price-key';

const priceArchiveConsumerOne = async (
  channel: AMQPChannel,
  queueName: string,
  queueParams: QueueParams
) => {
  try {
    const queue = await channel.queue(queueName, queueParams);
+   await queue.bind(exchangeName, routingKey);
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
-       // message.ack();
+       message.ack();
      }
    );
    console.log('Price archive consumer 1 configured');
  } catch (error) {
    console.error(error);
  }
};

const priceArchiveConsumerTwo = async (
  channel: AMQPChannel,
  queueName: string,
  queueParams: QueueParams
) => {
  try {
    const queue = await channel.queue(queueName, queueParams);
+   await queue.bind(exchangeName, routingKey);
    await queue.subscribe(
...

  await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
  const channel = await messageBroker.channel(2);
  channel.prefetch(1);
+ await channel.exchangeDeclare(exchangeName, 'direct', { durable: true });
  const queueName = 'price-archive-queue';
...

```

Ejecutamos un POST para ver que todo siguien funcionando:

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

> OJO: De momento tenemos el mismo comportamiento que antes, pero ahora tenemos un `exchange` explícito.

Ahora viene la parte interesantes, vamos a crear dos consumidores que se suscriban a este `exchange` y que enruten los mensajes en función del precio del libro:

Imaginate que para pedidos de menos de 100 € utilizamos correos express (y se gestiona por un microservicio que se inntegra con la API de correos), y para pedidos de más de esa cantidad lo enviamos por mensajería urgente (y se gestiona por otro microservicio que se integra con la API de la mensajería urgente).

_./back/src/pods/book/book.rest-api.ts_

```diff
...

const sendBookToArchive = async (book: Book) => {
  const exchangeName = 'price-archive';
- const routingKey = 'price-key';
+ const routingKey = book.price <= 100 ? 'low-prices' : 'high-prices';
  const channel = await messageBroker.channel(1);
  await channel.basicPublish(exchangeName, routingKey, JSON.stringify(book), {
    deliveryMode: 2,
  });
};

...

Ahora podemos crear un consumidor que se suscriba a la cola de bajo precio y otro los de más de 100 €.

```

_./consumers/src/index.ts_

```diff
- import { AMQPChannel, QueueParams } from '@cloudamqp/amqp-client';
+ import { AMQPChannel } from '@cloudamqp/amqp-client';
import { envConstants } from 'core/constants';

...

const exchangeName = 'price-archive';
- const routingKey = 'price-key';

const priceArchiveConsumerOne = async (
  channel: AMQPChannel,
- queueName: string,
- queueParams: QueueParams
) => {
  try {
-   const queue = await channel.queue(queueName, queueParams);
+   const queue = await channel.queue('low-prices-queue', { durable: true });
-   await queue.bind(exchangeName, routingKey);
+   await queue.bind(exchangeName, 'low-prices');
...

const priceArchiveConsumerTwo = async (
  channel: AMQPChannel,
- queueName: string,
- queueParams: QueueParams
) => {
  try {
-   const queue = await channel.queue(queueName, queueParams);
+   const queue = await channel.queue('high-prices-queue', { durable: true });
-   await queue.bind(exchangeName, routingKey);
+   await queue.bind(exchangeName, 'high-prices');
    ...
};
...

await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
const channel = await messageBroker.channel(2);
channel.prefetch(1);
await channel.exchangeDeclare(exchangeName, 'direct', { durable: true });
- const queueName = 'price-archive-queue';
- const queueParams: QueueParams = { durable: true };
- await priceArchiveConsumerOne(channel, queueName, queueParams);
+ await priceArchiveConsumerOne(channel);
- await priceArchiveConsumerTwo(channel, queueName, queueParams);
+ await priceArchiveConsumerTwo(channel)

```

Si ejecutamos la aplicación, podemos probar con varios mensajes y ver como funciona el enrutado:
Run app and send some message:

- Precio 20 varios veces
- Precio 100
- Precio 101

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
