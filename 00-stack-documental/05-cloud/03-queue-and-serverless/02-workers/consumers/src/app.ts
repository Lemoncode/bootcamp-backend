import { AMQPChannel, QueueParams } from '@cloudamqp/amqp-client';
import { envConstants } from 'core/constants';
import { connectToMessageBrokerServer, messageBroker } from 'core/servers';

const run = async () => {
  await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
  const channel = await messageBroker.channel(2);
  channel.prefetch(1);
  const queueName = 'price-archive-queue';
  const queueParams: QueueParams = { durable: true };
  await priceArchiveConsumerOne(channel, queueName, queueParams);
  await priceArchiveConsumerTwo(channel, queueName, queueParams);
};

const priceArchiveConsumerOne = async (
  channel: AMQPChannel,
  queueName: string,
  queueParams: QueueParams
) => {
  try {
    const queue = await channel.queue(queueName, queueParams);
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
        // message.ack();
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

run();
