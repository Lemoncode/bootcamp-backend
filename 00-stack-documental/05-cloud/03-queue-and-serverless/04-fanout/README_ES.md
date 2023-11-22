# 04 Fanout

En este ejemplo vamos a configurar el broker de mensajes como fanout.

Vamos a partir de `03-routing`.

# Pasos para construirlo

`npm install` para instalar las librerías necesarias:

```bash
cd back
npm install

cd consumers
npm install

```

Si queremos que todos los workers reciban todos los mensajes:

_./back/src/pods/book/book.rest-api.ts_

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

Actualizamos los _consumers_:

_./consumers/src/index.ts_

> Tenemos que borrar el exchange actual en el RabbitMQ Server Management
>
> Nombres de colas autogenerados.
>
> exclusive: borrará la cola al cerrar la conexión.

```diff
...

const exchangeName = 'price-archive';

const priceArchiveConsumerOne = async (channel: AMQPChannel) => {
  try {
-   const queue = await channel.queue('low-prices-queue', { durable: true });
+   const queue = await channel.queue('', { exclusive: true });
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
+   const queue = await channel.queue('', { exclusive: true });
-   await queue.bind(exchangeName, 'high-prices');
+   await queue.bind(exchangeName);
    await queue.subscribe(
      {
        noAck: false,
      },
...

await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
const channel = await messageBroker.channel(2);
channel.prefetch(1);
- channel.exchangeDeclare(exchangeName, 'direct', { durable: true });
+ channel.exchangeDeclare(exchangeName, 'fanout', { durable: true });
await priceArchiveConsumerOne(channel);
await priceArchiveConsumerTwo(channel);

```

> Actualizar 'fanout' en exchange book.rest.api --> sendBookToArchive

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

> Cierre el proceso `consumers` para ver cómo se eliminan las colas