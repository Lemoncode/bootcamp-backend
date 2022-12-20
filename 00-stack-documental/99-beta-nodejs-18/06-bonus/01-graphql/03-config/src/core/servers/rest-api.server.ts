import express from 'express';
import cookieParser from 'cookie-parser';

export const createRestApiServer = () => {
  const restApiServer = express();
  restApiServer.use(express.json());
  restApiServer.use(cookieParser());

  return restApiServer;
};
