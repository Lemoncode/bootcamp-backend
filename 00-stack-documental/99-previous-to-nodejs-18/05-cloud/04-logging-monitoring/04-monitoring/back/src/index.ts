import { config } from 'dotenv';
config();

const { envConstants } = require('./core/constants');
if (envConstants.isProduction) {
  require('newrelic');
}

require('./app');
