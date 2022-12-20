import { RequestHandler, ErrorRequestHandler } from "express";

export const logRequestMiddleware: RequestHandler = async (req, res, next) => {
  console.log(req.url);
  next();
};

export const logErrorRequestMiddleware: ErrorRequestHandler = async (
  error,
  req,
  res,
  next
) => {
  console.error(error);
  res.sendStatus(500);
};
