import { messageBroker } from '#core/servers/index.js';

try {
  await messageBroker.connect();
  const channel = await messageBroker.channel();
  const queue = await channel.queue('hello-queue', { durable: false });

  await queue.subscribe(
    {
      noAck: true,
    },
    (message) => {
      console.log(message.bodyToString());
    }
  );
  console.log('Hello consumer configured');
} catch (error) {
  console.error(error);
}
