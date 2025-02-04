import { createLogger } from 'winston';
import Transport from 'winston-transport';
import { ENV } from '#core/constants/index.js';
import { console, file, rollbar } from './transports/index.js';

let transports: Transport[] = [console, file];
if (ENV.IS_PRODUCTION) {
  transports = [...transports, rollbar];
}

export const logger = createLogger({
  transports,
});
