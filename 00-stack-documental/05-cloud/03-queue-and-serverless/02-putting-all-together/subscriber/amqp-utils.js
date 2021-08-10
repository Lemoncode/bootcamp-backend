const amqp = require('amqplib');

const url = 'amqp://guest:guest@localhost:5672';

let connection;

module.exports.openChannel = async () => {
    connection = await amqp.connect(url);
    return connection.createChannel();
};

module.exports.close = async () => {
    await connection.close();
};
