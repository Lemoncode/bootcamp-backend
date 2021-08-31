import { messageBrokerService, invokeWithDelay } from "../utils";

const { close, openChannel } = messageBrokerService();

const run = async () => {
  try {
    const channel = await openChannel();
    const exchange = "logs";
    const msg = process.argv.slice(2).join(" ");

    await channel.assertExchange(exchange, "fanout", { durable: false });
    channel.publish(exchange, "", Buffer.from(msg));
    
    console.log(`[x] Sent ${msg}`);

    invokeWithDelay(500, close);
  } catch (error) {
      console.error(error);
  }
};

run();
