import { AMQPClient } from '@cloudamqp/amqp-client';
import { AMQPBaseClient } from '@cloudamqp/amqp-client/types/amqp-base-client';

export let messageBroker: AMQPBaseClient;

export const connectToMessageBrokerServer = async (connectionURI: string) => {
  const client = new AMQPClient(connectionURI);
  messageBroker = await client.connect();
};
