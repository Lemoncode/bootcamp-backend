import path from 'node:path';
import { Express } from 'express';
import { createHandler, HandlerOptions } from 'graphql-http/lib/use/express';
import { ENV } from '#core/constants/index.js';

export const createGraphqlServer = (
  expressApp: Express,
  options: HandlerOptions
) => {
  expressApp.use('/graphql', createHandler(options));
  if (!ENV.IS_PRODUCTION) {
    expressApp.use('/playground', async (req, res) => {
      res.sendFile(
        path.join(import.meta.dirname, '../graphql/playground.html')
      );
    });
  }
};
