import '#core/load-env.js';
import { AMQPChannel } from '@cloudamqp/amqp-client';
import { envConstants } from '#core/constants/index.js';
import {
  connectToMessageBrokerServer,
  messageBroker,
} from '#core/servers/index.js';

const exchangeName = 'price-archive';

const priceArchiveConsumerOne = async (channel: AMQPChannel) => {
  try {
    const queue = await channel.queue('low-prices-queue', { durable: true });
    await queue.bind(exchangeName, 'low-prices');
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
        message.ack();
      }
    );
    console.log('Price archive consumer 1 configured');
  } catch (error) {
    console.error(error);
  }
};

const priceArchiveConsumerTwo = async (channel: AMQPChannel) => {
  try {
    const queue = await channel.queue('high-prices-queue', { durable: true });
    await queue.bind(exchangeName, 'high-prices');
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

await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
const channel = await messageBroker.channel(2);
channel.prefetch(1);
channel.exchangeDeclare(exchangeName, 'direct', { durable: true });
await priceArchiveConsumerOne(channel);
await priceArchiveConsumerTwo(channel);
