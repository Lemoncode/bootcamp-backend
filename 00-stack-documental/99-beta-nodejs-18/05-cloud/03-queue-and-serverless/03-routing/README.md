# 03 Routing

In this example we are going to configure the message broker as a router.

We will start from `02-workers`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
cd back
npm install

cd consumers
npm install

```

The previous example was using the default AMQP `exchange`, that is, we will update the example but using an explicit one:

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
+ await channel.basicPublish(exchangeName, routingKey, JSON.stringify(book), {
+   deliveryMode: 2,
+ });
};

...

```

Update consumers:

_./consumers/src/app.ts_

```diff
...

+ const exchangeName = 'price-archive';
+ const routingKey = 'price-key';

const run = async () => {
  await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
  const channel = await messageBroker.channel(2);
  channel.prefetch(1);
+ channel.exchangeDeclare(exchangeName, 'direct', { durable: true });
  const queueName = 'price-archive-queue';
...
};

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

```

The `routingKey`s are usefull if we want to route messages, for example, route by max price:

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

```

_./consumers/src/app.ts_

```diff
- import { AMQPChannel, QueueParams } from '@cloudamqp/amqp-client';
+ import { AMQPChannel } from '@cloudamqp/amqp-client';
import { envConstants } from 'core/constants';

...

const exchangeName = 'price-archive';
- const routingKey = 'price-key';

const run = async () => {
  await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
  const channel = await messageBroker.channel(2);
  channel.prefetch(1);
  channel.exchangeDeclare(exchangeName, 'direct', { durable: true });
- const queueName = 'price-archive-queue';
- const queueParams: QueueParams = { durable: true };
- await priceArchiveConsumerOne(channel, queueName, queueParams);
+ await priceArchiveConsumerOne(channel);
- await priceArchiveConsumerTwo(channel, queueName, queueParams);
+ await priceArchiveConsumerTwo(channel);
};

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
    await queue.subscribe(
      {
        noAck: false,
      },
      (message) => {
        console.log('Worker 2 message received');
        const book = JSON.parse(message.bodyToString());
        console.log(
          `Saving book with title "${book.title}" and price ${book.price}`
        );
        message.ack();
      }
    );
    console.log('Price archive consumer 2 configured');
  } catch (error) {
    console.error(error);
  }
};
...
```

Run app and send some message:

- Price 20 multiple times
- Price 100
- Price 101

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
