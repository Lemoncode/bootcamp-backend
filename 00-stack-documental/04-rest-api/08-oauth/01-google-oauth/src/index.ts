import './load-env.js';
import express from 'express';
import path from 'path';
import url from 'url';
import { createApp } from './express.server.js';
import { envConstants } from './env.constants.js';
import { api } from './api.js';
import cookieParser from 'cookie-parser';
import passport from 'passport';
import { configPassport } from './setup/index.js';

const app = createApp();
app.use(cookieParser());

configPassport(passport);
// We need to setup the middleware
app.use(passport.initialize());

const __dirname = path.dirname(url.fileURLToPath(import.meta.url));
const staticFilesPath = path.resolve(__dirname, envConstants.STATIC_FILES_PATH);
app.use('/', express.static(staticFilesPath));

app.use('/api', api);

app.listen(envConstants.PORT, () => {
  console.log(`Server ready at http://localhost:${envConstants.PORT}/api`);
});