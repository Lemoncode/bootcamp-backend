import { createLogger } from 'winston';
import { console, file } from './transports';

export const logger = createLogger({
  transports: [console, file],
});
