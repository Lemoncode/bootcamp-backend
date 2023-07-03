import '#core/load-env.js';
import { envConstants } from '#core/constants/index.js';
import {
  connectToMessageBrokerServer,
  messageBroker,
} from '#core/servers/index.js';

try {
  await connectToMessageBrokerServer(envConstants.RABBITMQ_URI);
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
