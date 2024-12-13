import { AMQPChannel } from '@cloudamqp/amqp-client';
import { messageBroker } from '#core/servers/index.js';

const exchangeName = 'price-archive';

const priceArchiveConsumerOne = async (channel: AMQPChannel) => {
  try {
    const queue = await channel.queue('', { autoDelete: true, exclusive: true });
    await queue.bind(exchangeName);
    await queue.subscribe(
      {
        noAck: false,
      },
      (message) => {
        console.log('**** Worker 1 processing message ****');
        const book = JSON.parse(message.bodyToString());
        console.log(
          `Saving book with title "${book.title}" and price ${book.price}`
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
    const queue = await channel.queue('', { autoDelete: true, exclusive: true });
    await queue.bind(exchangeName);
    await queue.subscribe(
      {
        noAck: false,
      },
      (message) => {
        console.log('**** Worker 2 processing message ****');
        const book = JSON.parse(message.bodyToString());
        console.log(
          `Saving book with title "${book.title}" and price ${book.price}`
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
await channel.exchangeDeclare(exchangeName, 'fanout', { durable: true });
await priceArchiveConsumerOne(channel);
await priceArchiveConsumerTwo(channel);
