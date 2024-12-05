import express from 'express';
import cors from 'cors';
import cookieParser from 'cookie-parser';
import { ENV } from '../constants/index.js';

export const createRestApiServer = () => {
  const app = express();
  app.use(express.json());
  app.use(
    cors({
      methods: ENV.CORS_METHODS,
      origin: ENV.CORS_ORIGIN,
    })
  );
  app.use(cookieParser());

  return app;
};
