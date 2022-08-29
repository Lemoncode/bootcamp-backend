import { messageBrokerService, invokeWithDelay } from "../utils";

const { openChannel } = messageBrokerService();

const run = async () => {
  try {
    const channel = await openChannel();
    const exchange = "logs";
    await channel.assertExchange(exchange, "fanout", { durable: false });
    const { queue } = await channel.assertQueue("");
    console.log(`[*] waiting for messages in ${queue}`);
    await channel.bindQueue(queue, exchange, "");
    channel.consume(
      queue,
      (msg) => {
        const { content } = msg;
        if (content) {
          console.log(`[x] ${content.toString()}`);
        }
      },
      { noAck: true } // [1]
    );
  } catch (error) {
    console.error(error);
  }
};

run();

// [1].(boolean): if true, the broker won’t expect an acknowledgement of messages delivered to this consumer; i.e., it will dequeue messages as soon as they’ve been sent down the wire.
