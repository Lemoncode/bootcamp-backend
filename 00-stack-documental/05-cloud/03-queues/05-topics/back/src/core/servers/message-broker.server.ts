import { AMQPClient } from '@cloudamqp/amqp-client';
import { ENV } from '#core/constants/index.js';

export const messageBroker = new AMQPClient(ENV.RABBITMQ_URL);
