export const envConstants = {
  isProduction: process.env.NODE_ENV === 'production',
  RABBITMQ_URI: process.env.RABBITMQ_URI,
};
