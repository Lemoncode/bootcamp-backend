import { messageBrokerService, invokeWithDelay } from "../utils";

const { openChannel } = messageBrokerService();

const run = async () => {
  try {
    const channel = await openChannel();
    const queue = "task_queue";
    await channel.assertQueue(queue, { durable: true });
    
    channel.prefetch(1);

    channel.consume(
      queue,
      async (msg) => {
        const { content } = msg;
        const seconds = content.toString().split(".").length - 1;
        console.log(`[x] Received ${content.toString()}`);

        await invokeWithDelay(seconds * 1_000, () => {
          console.log("[x] Done");
          channel.ack(msg);
        });
      },
      { noAck: false }
    );
  } catch (error) {
    console.error(error);
  }
};

run();
