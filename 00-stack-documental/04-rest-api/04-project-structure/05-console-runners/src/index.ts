import express from "express";
import path from "node:path";
import {
  logRequestMiddleware,
  logErrorRequestMiddleware,
} from "#common/middlewares/index.js";
import { createRestApiServer } from "#core/servers/index.js";
import { ENV } from "#core/constants/index.js";
import { bookApi } from "#pods/book/index.js";

const app = createRestApiServer();

app.use(
  "/",
  express.static(path.resolve(import.meta.dirname, ENV.STATIC_FILES_PATH))
);

app.use(logRequestMiddleware);

app.use("/api/books", bookApi);

app.use(logErrorRequestMiddleware);

app.listen(ENV.PORT, () => {
  console.log(`Server ready at port ${ENV.PORT}`);
});
