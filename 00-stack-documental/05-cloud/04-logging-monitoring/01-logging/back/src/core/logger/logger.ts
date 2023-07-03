import { createLogger } from 'winston';
import { console, file } from './transports/index.js';

export const logger = createLogger({
  transports: [console, file],
});
