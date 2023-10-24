# 05 Topics

En este ejemplo vamos a configurar el broker de mensajes usando topics.

Vamos a partir de `04-fanout`.

# Pasos para construirlo

`npm install` para instalar las librerías necesarias:

```bash
cd back
npm install

cd consumers
npm install

```

Esta vez queremos recibir información de varios topics `<price>.<author>.<date>`:

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

await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
const channel = await messageBroker.channel(2);
channel.prefetch(1);
- await channel.exchangeDeclare(exchangeName, 'fanout', { durable: true });
+ await channel.exchangeDeclare(exchangeName, 'topic', { durable: true });
await priceArchiveConsumerOne(channel);
await priceArchiveConsumerTwo(channel);

```
> Tenemos que borrar el exchange actual en el RabbitMQ Server Management
>
> `high-prices.#` es similar a `high-prices.*.*`
>
> `#.old` es similar a `*.*.old`
>
> `#` elige cualquier mensaje

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
> `low-prices.John Doe.new`: Sólo el worker 1 recibirá este mensaje	

- Price: `200`
> `high-prices.John Doe.new`: Sólo el worker 2 recibirá este mensaje	

- Price: `90`, Author: `John Doe` y releaseDate: `1990`
> `low-prices.John Doe.old`: Sólo el worker 2 recibirá este mensaje

- Price: `90`, Author: `Jane Doe` y releaseDate: `1990`
> `low-prices.Jane Doe.old`: los workers 1 y 2 recibirán este mensaje

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).