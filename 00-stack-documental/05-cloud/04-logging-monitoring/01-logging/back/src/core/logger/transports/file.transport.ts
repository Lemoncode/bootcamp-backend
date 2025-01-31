import { transports, format } from 'winston';

const { combine, timestamp, prettyPrint } = format;

export const file = new transports.File({
  filename: 'app.log',
  format: combine(timestamp(), prettyPrint()),
  level: 'warn', // Save level lower or equal than warning
  handleExceptions: true,
});
