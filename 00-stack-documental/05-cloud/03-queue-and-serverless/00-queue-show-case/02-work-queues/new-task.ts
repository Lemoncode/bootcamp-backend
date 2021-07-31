import { messageBrokerService, invokeWithDelay } from '../utils';

const { close, openChannel } = messageBrokerService();

const run = async () => {
    try {
        const channel = await openChannel();
        const queue = 'task_queue';
        const msg = process.argv.slice(2).join(' ');

        await channel.assertQueue(queue, { durable: true }); // [1]
        channel.sendToQueue(queue, Buffer.from(msg), { persistent: true }); // [2]
        
        console.log(`[x] Sent ${msg}`);
        
        invokeWithDelay(1_000, close);
    } catch (error) {
        console.error(error);
    }
};

run();

// [1]. (Boolean)if true, the queue will survive broker restarts, modulo the effects of
// [2]. If truthy, the message will survive broker restarts provided it’s in a queue that also survives restarts. Corresponds to, and overrides, the property deliveryMode.
