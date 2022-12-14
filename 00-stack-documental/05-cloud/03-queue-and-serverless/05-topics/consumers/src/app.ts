import { AMQPChannel } from '@cloudamqp/amqp-client';
import { envConstants } from 'core/constants';
import { connectToMessageBrokerServer, messageBroker } from 'core/servers';

const exchangeName = 'price-archive';

const run = async () => {
  await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
  const channel = await messageBroker.channel(2);
  channel.prefetch(1);
  channel.exchangeDeclare(exchangeName, 'topic', { durable: true, });
  await priceArchiveConsumerOne(channel);
  await priceArchiveConsumerTwo(channel);
};

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
        console.log('Worker 1 message received');
        const book = JSON.parse(message.bodyToString());
        console.log(
          `Saving book with price ${book.price}, author ${book.author} and year ${book.releaseDate}`
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
    const queue = await channel.queue('', { exclusive: true });
    await queue.bind(exchangeName, 'high-prices.#');
    await queue.bind(exchangeName, '#.old');
    await queue.subscribe(
      {
        noAck: false,
      },
      (message) => {
        console.log('Worker 2 message received');
        const book = JSON.parse(message.bodyToString());
        console.log(
          `Saving book with price ${book.price}, author ${book.author} and year ${book.releaseDate}`
        );
        message.ack();
      }
    );
    console.log('Price archive consumer 2 configured');
  } catch (error) {
    console.error(error);
  }
};

run();
