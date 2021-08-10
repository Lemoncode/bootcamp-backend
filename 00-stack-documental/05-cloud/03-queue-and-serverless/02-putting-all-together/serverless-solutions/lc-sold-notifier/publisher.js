const { openChannel, close } = require('./amqp-utils');

module.exports = class {
    constructor(exchange, type) {
        this.channel = null;
        this.exchange = exchange;
        this.type = type;
    }

    async startConnection() {
        try {
            this.channel = await openChannel();
        } catch (error) {
            console.log(error);
            throw error;
        }
    }

    async assertExchange(options) {
        if (!this.channel) {
            throw 'Not channel initialised, you must open a channel before use this method.';
        }

        try {
            await this.channel.assertExchange(this.exchange, this.type, options);
        } catch (error) {
            console.log(error);
            throw error;
        }
    }

    publish(msg) {
        try {
            this.channel.publish(this.exchange, '', Buffer.from(msg));
        } catch (error) {
            console.log(error);
            throw error;
        }
    }

    async close() {
        await close();
    }
};
