# 04 Fanout

In this example we are going to configure the message broker as fanout.

We will start from `03-routing`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
cd back
npm install

cd consumers
npm install

```

If we want all the workers to get all the messages:

_./back/src/pods/book/book.api.ts_

```diff
...
const sendBookToArchive = async (book: Book) => {
  const exchangeName = 'price-archive';
- const routingKey = book.price <= 100 ? 'low-prices' : 'high-prices';
+ const routingKey = '';
  const channel = await messageBroker.channel(1);
  await channel.basicPublish(exchangeName, routingKey, JSON.stringify(book), {
    deliveryMode: 2,
  });
};
...

```

Update consumers:

_./consumers/src/index.ts_

> We have to delete current exchange in RabbitMQ Server Management
>
> Autogenerated queue names.
>
> autoDelete: it will delete the queue after close consumers.
>
> exclusive: it will only available for one conection and also it will delete the queue after close connection.

```diff
...

const exchangeName = 'price-archive';

const priceArchiveConsumerOne = async (channel: AMQPChannel) => {
  try {
-   const queue = await channel.queue('low-prices-queue', { durable: true });
+   const queue = await channel.queue('', { autoDelete: true, exclusive: true });
-   await queue.bind(exchangeName, 'low-prices');
+   await queue.bind(exchangeName);
    await queue.subscribe(
      {
        noAck: false,
      },
...

const priceArchiveConsumerTwo = async (channel: AMQPChannel) => {
  try {
-   const queue = await channel.queue('high-prices-queue', { durable: true });
+   const queue = await channel.queue('', { autoDelete: true, exclusive: true });
-   await queue.bind(exchangeName, 'high-prices');
+   await queue.bind(exchangeName);
    await queue.subscribe(
      {
        noAck: false,
      },
...

await connectToMessageBrokerServer(envConstants.RABBITMQ_URL);
const channel = await messageBroker.channel(2);
channel.prefetch(1);
- channel.exchangeDeclare(exchangeName, 'direct', { durable: true });
+ channel.exchangeDeclare(exchangeName, 'fanout', { durable: true });
await priceArchiveConsumerOne(channel);
await priceArchiveConsumerTwo(channel);

```

> Update 'fanout' in exchange declared book.rest.api --> sendBookToArchive

Run POST method again:

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

> Close `consumers` process to see how to queues are deleted

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
