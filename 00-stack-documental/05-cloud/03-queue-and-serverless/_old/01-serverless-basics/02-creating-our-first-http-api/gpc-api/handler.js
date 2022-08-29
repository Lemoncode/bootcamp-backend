'use strict';

const { DynamoDBClient, PutItemCommand } = require('@aws-sdk/client-dynamodb');

const client = new DynamoDBClient({ region: 'eu-west-2' });

module.exports.handler = async (event) => {
  try {
    const params = {
      TableName: 'users',
      Item: {
        'email': { 'S': 'jaime.salas@lemoncode.net' }
      },
    };

    const command = new PutItemCommand(params);
    await client.send(command);

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
  } catch (error) {
    console.error(error);
  }
};
