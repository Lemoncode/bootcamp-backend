import { envConstants } from '#core/constants/index.js';

if (envConstants.isProduction) {
  import('newrelic');
}
