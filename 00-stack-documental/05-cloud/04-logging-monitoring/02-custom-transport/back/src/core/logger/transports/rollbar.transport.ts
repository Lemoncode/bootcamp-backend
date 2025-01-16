import { format } from 'winston';
import { RollbarTransport } from '#common/logger-transports/index.js';
import { ENV } from '#core/constants/index.js';

const { combine, timestamp, prettyPrint } = format;

export const rollbar = new RollbarTransport({
  accessToken: ENV.ROLLBAR_ACCESS_TOKEN,
  environment: ENV.ROLLBAR_ENV,
  captureUncaught: ENV.IS_PRODUCTION,
  captureUnhandledRejections: ENV.IS_PRODUCTION,
  format: combine(timestamp(), prettyPrint()),
  level: 'warn',
});
