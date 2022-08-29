'use strict';

const Publisher = require('./publisher');

module.exports.sold = async (event) => {
  const { body } = event;
  const publisher = new Publisher('sales', 'fanout');
  await publisher.startConnection();
  await publisher.assertExchange({durable: false});
  publisher.publish(body);
  await publisher.close();
  
  return {
    statusCode: 200,
    body: JSON.stringify(
      {
        message: 'message send to queue',
      },
      null,
      2
    ),
  };
};
