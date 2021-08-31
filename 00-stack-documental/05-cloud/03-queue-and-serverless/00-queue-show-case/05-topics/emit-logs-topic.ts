import { messageBrokerService, invokeWithDelay } from "../utils";

const { close, openChannel } = messageBrokerService();

const run = async () => {
  try {
    const channel = await openChannel();
    const exchange = "topic_logs";
    const args = process.argv.slice(2);
    const key = (args.length > 0) ? args[0] : 'anonymous.info';
    const msg = args.slice(1).join(" ");
    
    await channel.assertExchange(exchange, "topic", { durable: false });
    
    channel.publish(exchange, key, Buffer.from(msg));
    
    console.log(`[x] Sent ${key} ${msg}`);

    invokeWithDelay(500, close);
  } catch (error) {
    console.error(error);
  }
};

run();
