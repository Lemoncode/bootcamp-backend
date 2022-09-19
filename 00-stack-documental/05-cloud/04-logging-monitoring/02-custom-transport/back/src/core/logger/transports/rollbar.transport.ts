import { format } from 'winston';
import { RollbarTransport } from 'common/logger-transports';
import { envConstants } from 'core/constants';

const { combine, timestamp, prettyPrint } = format;

export const rollbar = new RollbarTransport({
  accessToken: envConstants.ROLLBAR_ACCESS_TOKEN,
  environment: envConstants.ROLLBAR_ENV,
  captureUncaught: envConstants.isProduction,
  captureUnhandledRejections: envConstants.isProduction,
  format: combine(timestamp(), prettyPrint()),
  level: 'warn',
});
