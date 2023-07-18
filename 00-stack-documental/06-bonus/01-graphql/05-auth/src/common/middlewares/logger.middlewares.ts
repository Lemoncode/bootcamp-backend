import { RequestHandler, ErrorRequestHandler } from 'express';
import { Logger } from 'winston';

export const logRequestMiddleware =
  (logger: Logger): RequestHandler =>
  async (req, res, next) => {
    logger.info(req.url);
    next();
  };

export const logErrorRequestMiddleware =
  (logger: Logger): ErrorRequestHandler =>
  async (error, req, res, next) => {
    logger.error(error.stack);
    res.sendStatus(500);
  };
