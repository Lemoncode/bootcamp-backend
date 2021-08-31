const { openChannel } = require('./amqp-utils');

const run = async () => { 
    try {
        const channel = await openChannel();
        const exchange = 'sales';
        await channel.assertExchange(exchange, 'fanout', { durable: false});
        const { queue } = await channel.assertQueue('');
        await channel.bindQueue(queue, exchange, '');
        channel.consume(
            queue,
            (msg) => {
                const { content } = msg;
                console.log(content.toString());
                channel.ack(msg);
            }
        );
    } catch (error) {
        
    }
};

run();
