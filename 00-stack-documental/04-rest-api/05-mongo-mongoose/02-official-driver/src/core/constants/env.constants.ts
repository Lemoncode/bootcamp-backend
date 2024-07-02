export const ENV = {
  IS_PRODUCTION: process.env.NODE_ENV === "production",
  PORT: Number(process.env.PORT),
  STATIC_FILES_PATH: process.env.STATIC_FILES_PATH,
  CORS_ORIGIN: process.env.CORS_ORIGIN,
  CORS_METHODS: process.env.CORS_METHODS,
  IS_API_MOCK: process.env.IS_API_MOCK === "true",
  MONGODB_URL: process.env.MONGODB_URL,
};
