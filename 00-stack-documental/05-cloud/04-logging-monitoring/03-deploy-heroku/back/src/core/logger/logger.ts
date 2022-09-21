import { createLogger } from 'winston';
import Transport from 'winston-transport';
import { envConstants } from 'core/constants';
import { console, file, rollbar } from './transports';

let transports: Transport[] = [console, file];
if (envConstants.isProduction) {
  transports = [...transports, rollbar];
}

export const logger = createLogger({
  transports,
});
