import express from "express";
import cors from "cors";
import { envConstants } from "../constants/index.js";

export const createRestApiServer = () => {
  const restApiServer = express();
  restApiServer.use(express.json());
  restApiServer.use(
    cors({
      methods: envConstants.CORS_METHODS,
      origin: envConstants.CORS_ORIGIN,
      credentials: true,
    })
  );

  return restApiServer;
};
