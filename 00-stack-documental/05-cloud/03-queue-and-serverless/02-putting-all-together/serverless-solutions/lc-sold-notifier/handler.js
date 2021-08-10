'use strict';

module.exports.sold = async (event) => {
  const { body } = event;
  console.log(body);
  return {
    statusCode: 200,
    body: JSON.stringify(
      {
        message: 'Go Serverless v2.0! Your function executed successfully!',
        input: event,
      },
      null,
      2
    ),
  };
};
