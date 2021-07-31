import { messageBrokerService, invokeWithDelay } from "../utils";

const { close, openChannel } = messageBrokerService();

const run = async () => {
  try {
    const channel = await openChannel();
    const exchange = "direct_logs";
    const args = process.argv.slice(2);
    const msg = args.slice(1).join(" ");
    const severity = args.length > 0 ? args[0] : "info";

    await channel.assertExchange(exchange, "direct", { durable: false });
    
    channel.publish(exchange, severity, Buffer.from(msg));
    
    console.log(`[x] Sent ${severity} ${msg}`);

    invokeWithDelay(500, close);
  } catch (error) {
    console.error(error);
  }
};

run();
