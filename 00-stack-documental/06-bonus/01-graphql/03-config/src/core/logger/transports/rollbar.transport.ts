import { format } from 'winston';
import { RollbarTransport } from '#common/logger-transports/index.js';
import { envConstants } from '#core/constants/index.js';

const { combine, timestamp, prettyPrint } = format;

export const rollbar = new RollbarTransport({
  accessToken: envConstants.ROLLBAR_ACCESS_TOKEN,
  environment: envConstants.ROLLBAR_ENV,
  captureUncaught: envConstants.isProduction,
  captureUnhandledRejections: envConstants.isProduction,
  format: combine(timestamp(), prettyPrint()),
  level: 'warn',
});
