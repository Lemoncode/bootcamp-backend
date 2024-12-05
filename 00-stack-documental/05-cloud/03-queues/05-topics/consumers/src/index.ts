import { AMQPChannel } from '@cloudamqp/amqp-client';
import { messageBroker } from '#core/servers/index.js';

const exchangeName = 'price-archive';

const priceArchiveConsumerOne = async (channel: AMQPChannel) => {
  try {
    const queue = await channel.queue('', { exclusive: true });
    await queue.bind(exchangeName, 'low-prices.*.new');
    await queue.bind(exchangeName, '*.Jane Doe.*');
    await queue.subscribe(
      {
        noAck: false,
      },
      (message) => {
        console.log('**** Worker 1 processing message ****');
        const book = JSON.parse(message.bodyToString());
        console.log(
          `Saving book with price ${book.price}, author ${book.author} and year ${book.releaseDate}`
        );
        message.ack();
      }
    );
    console.log('**** Worker 1 ready ****');
  } catch (error) {
    console.error(error);
  }
};

const priceArchiveConsumerTwo = async (channel: AMQPChannel) => {
  try {
    const queue = await channel.queue('', { exclusive: true });
    await queue.bind(exchangeName, 'high-prices.#');
    await queue.bind(exchangeName, '#.old');
    await queue.subscribe(
      {
        noAck: false,
      },
      (message) => {
        console.log('**** Worker 2 processing message ****');
        const book = JSON.parse(message.bodyToString());
        console.log(
          `Saving book with price ${book.price}, author ${book.author} and year ${book.releaseDate}`
        );
        message.ack();
      }
    );
    console.log('**** Worker 2 ready ****');
  } catch (error) {
    console.error(error);
  }
};

await messageBroker.connect();
const channel = await messageBroker.channel();
await channel.prefetch(1);
await channel.exchangeDeclare(exchangeName, 'topic', { durable: true });
await priceArchiveConsumerOne(channel);
await priceArchiveConsumerTwo(channel);
