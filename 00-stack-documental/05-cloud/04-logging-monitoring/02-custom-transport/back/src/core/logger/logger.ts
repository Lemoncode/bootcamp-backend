import { createLogger } from 'winston';
import { console, file, rollbar } from './transports/index.js';

export const logger = createLogger({
  transports: [console, file, rollbar],
});
