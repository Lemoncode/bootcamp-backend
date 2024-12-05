import { AMQPChannel, QueueParams } from '@cloudamqp/amqp-client';
import { messageBroker } from '#core/servers/index.js';

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
        console.log('**** Worker 1 processing message ****');
        console.log('**** Looooong task, work in progress ****');
        // const book = JSON.parse(message.bodyToString());
        // console.log(
        //   `Saving book with title "${book.title}" and price ${book.price}`
        // );
        // message.ack();
      }
    );
    console.log('**** Worker 1 ready ****');
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
const queueName = 'price-archive-queue';
const queueParams: QueueParams = { durable: true };
await priceArchiveConsumerOne(channel, queueName, queueParams);
await priceArchiveConsumerTwo(channel, queueName, queueParams);
