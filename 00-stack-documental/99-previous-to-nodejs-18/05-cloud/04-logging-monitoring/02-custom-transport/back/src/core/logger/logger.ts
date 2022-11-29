import { createLogger } from 'winston';
import { console, file, rollbar } from './transports';

export const logger = createLogger({
  transports: [console, file, rollbar],
});
