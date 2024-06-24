import express from "express";
import path from "node:path";
import { createRestApiServer } from "#core/servers/index.js";
import { ENV } from "#core/constants/index.js";
import { bookApi } from "#pods/book/index.js";

const app = createRestApiServer();

app.use(
  "/",
  express.static(path.resolve(import.meta.dirname, ENV.STATIC_FILES_PATH))
);

app.use(async (req, res, next) => {
  console.log(req.url);
  next();
});

app.use("/api/books", bookApi);

app.use(async (error, req, res, next) => {
  console.error(error);
  res.sendStatus(500);
});

app.listen(ENV.PORT, () => {
  console.log(`Server ready at port ${ENV.PORT}`);
});
