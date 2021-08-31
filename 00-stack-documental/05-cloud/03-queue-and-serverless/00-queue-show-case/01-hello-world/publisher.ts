import { messageBrokerService, invokeWithDelay } from '../utils';

const { close, openChannel } = messageBrokerService();

const run = async () => {
    try {
        const channel = await openChannel();
        const queue = 'hello';
        const msg = 'Hello World';
        channel.assertQueue(queue, { durable: false }); // [1]
        channel.sendToQueue(queue, Buffer.from(msg));
        console.log(`[x] Sent ${msg}`);

        await invokeWithDelay(500, close);
    } catch (error) {
        console.error(error);
    }
};

run();

// [1]. if true, the queue will survive broker restarts, modulo the effects of
