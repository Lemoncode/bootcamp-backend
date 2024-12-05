# 05 Topics

In this example we are going to configure the message broker using topics.

We will start from `04-fanout`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
cd back
npm install

cd consumers
npm install

```

This time we want to receive info from several topics `<price>.<author>.<date>`:

_./consumers/src/index.ts_

```diff
...

const priceArchiveConsumerOne = async (channel: AMQPChannel) => {
  try {
    const queue = await channel.queue('', { exclusive: true });
-   await queue.bind(exchangeName);
+   await queue.bind(exchangeName, 'low-prices.*.new');
+   await queue.bind(exchangeName, '*.Jane Doe.*');
    await queue.subscribe(
      {
        noAck: false,
      },
      (message) => {
        console.log('Worker 1 message received');
        const book = JSON.parse(message.bodyToString());
-       console.log(
-         `Saving book with title "${book.title}" and price ${book.price}`
-       );
+       console.log(
+         `Saving book with price ${book.price}, author ${book.author} and year ${book.releaseDate}`
+       );
...

const priceArchiveConsumerTwo = async (channel: AMQPChannel) => {
  try {
    const queue = await channel.queue('', { exclusive: true });
-   await queue.bind(exchangeName);
+   await queue.bind(exchangeName, 'high-prices.#');
+   await queue.bind(exchangeName, '#.old');
    await queue.subscribe(
      {
        noAck: false,
      },
      (message) => {
        console.log('Worker 2 message received');
        const book = JSON.parse(message.bodyToString());
-       console.log(
-         `Saving book with title "${book.title}" and price ${book.price}`
-       );
+       console.log(
+         `Saving book with price ${book.price}, author ${book.author} and year ${book.releaseDate}`
+       );
...

await connectToMessageBrokerServer(envConstants.RABBITMQ_URL);
const channel = await messageBroker.channel(2);
channel.prefetch(1);
- await channel.exchangeDeclare(exchangeName, 'fanout', { durable: true });
+ await channel.exchangeDeclare(exchangeName, 'topic', { durable: true });
await priceArchiveConsumerOne(channel);
await priceArchiveConsumerTwo(channel);

```
> We have to delete current exchange in RabbitMQ Server Management
>
> `high-prices.#` is similar to `high-prices.*.*`
>
> `#.old` is similar to `*.*.old`
>
> `#` pick any message

_./back/src/pods/book/book.rest-api.ts_

```diff
...
const sendBookToArchive = async (book: Book) => {
  const exchangeName = 'price-archive';
+ const priceKey = book.price <= 100 ? 'low-prices' : 'high-prices';
+ const dateKey =
+   book.releaseDate.getFullYear() < new Date().getFullYear() ? 'old' : 'new';
- const routingKey = '';
+ const routingKey = `${priceKey}.${book.author}.${dateKey}`;
+ console.log({ routingKey });
  const channel = await messageBroker.channel(1);
  await channel.basicPublish(exchangeName, routingKey, JSON.stringify(book), {
    deliveryMode: 2,
  });
};
...

```

Queries:

```
URL: http://localhost:3000/api/books
METHOD: POST
BODY:
{
    "title": "My new book",
    "releaseDate": "2023-09-10T00:00:00.000Z",
    "author": "John Doe",
    "price": 90
}
```
> `low-prices.John Doe.new`: Only worker 1 will receive this message

- Price: `200`
> `high-prices.John Doe.new`: Only worker 2 will receive this message

- Price: `200` and Author: `Jane Doe`
> `high-prices.Jane Doe.new`: worker 1 and 2 will receive this message

- Price: `90`, Author: `John Doe` and releaseDate: `1990`
> `low-prices.John Doe.old`: Only worker 2 will receive this message

- Price: `90`, Author: `Jane Doe` and releaseDate: `1990`
> `low-prices.Jane Doe.old`: worker 1 and 2 will receive this message

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
