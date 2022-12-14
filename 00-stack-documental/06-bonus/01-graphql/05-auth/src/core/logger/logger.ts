import { createLogger } from 'winston';
import { console } from './transports';

export const logger = createLogger({
  transports: [console],
  exitOnError: false,
});
