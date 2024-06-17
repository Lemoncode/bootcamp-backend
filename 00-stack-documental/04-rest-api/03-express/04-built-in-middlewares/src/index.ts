import express from "express";
import path from "node:path";
import { booksApi } from "./books.api.js";

const app = express();
app.use(express.json());

app.use("/", express.static(path.resolve(import.meta.dirname, "../public")));

app.use("/api/books", booksApi);

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});
