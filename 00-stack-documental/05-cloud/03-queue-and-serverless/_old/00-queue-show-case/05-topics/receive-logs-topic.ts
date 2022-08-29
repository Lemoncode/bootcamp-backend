import { messageBrokerService } from "../utils";

const { openChannel } = messageBrokerService();

const run = async () => {
  try {
    const args = process.argv.slice(2);

    if (args.length === 0) {
      console.log(
        `Usage: $(npm bin)/ts-node 05-topics/receive-logs-topic.ts <facility>.<severity>`
      );
      process.exit(1);
    }

    const channel = await openChannel();
    const exchange = "topic_logs";
    await channel.assertExchange(exchange, "topic", { durable: false });
    const { queue } = await channel.assertQueue("");

    for (const key of args) {
      await channel.bindQueue(queue, exchange, key);
    }

    channel.consume(
      queue,
      (msg) => {
        console.log(`[x] ${msg.fields.routingKey} ${msg.content.toString()}`);
      },
      { noAck: true }
    );
  } catch (error) {
    console.error(error);
  }
};

run();
