import express from 'express';

export const createApp = () => {
  const app = express();
  app.use(express.json({
    verify: function (req, res, buf) {
      req['raw'] = buf;
    },
  }));

  return app;
};
