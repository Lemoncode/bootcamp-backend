import { RequestHandler, ErrorRequestHandler } from 'express';
import { logger } from '../logger';

export const logRequestMiddleware: RequestHandler = async (req, res, next) => {
  logger.info(req.url);
  next();
};

export const logErrorRequestMiddleware: ErrorRequestHandler = async (
  error,
  req,
  res,
  next
) => {
  logger.error(error.stack);
  res.sendStatus(500);
};
