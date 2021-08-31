import { messageBrokerService } from "../utils";

const { openChannel } = messageBrokerService();

const run = async () => {
  try {
    const args = process.argv.slice(2);

    if (args.length === 0) {
      console.log(
        `Usage: $(npm bin)/ts-node 04-routing/receive-logs-direct.ts [info] [warning] [error]`
      );
      process.exit(1);
    }

    const channel = await openChannel();
    const exchange = "direct_logs";
    await channel.assertExchange(exchange, "direct", { durable: false });
    const { queue } = await channel.assertQueue("");

    for (const severity of args) {
      await channel.bindQueue(queue, exchange, severity);
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
