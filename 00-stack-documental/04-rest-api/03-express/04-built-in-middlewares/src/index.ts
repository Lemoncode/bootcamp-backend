import express from "express";
import path from "path";
import url from "url";
import { booksApi } from "./books.api.js";

const app = express();
app.use(express.json());

// TODO: Feed env variable in production
const __dirname = path.dirname(url.fileURLToPath(import.meta.url));
app.use("/", express.static(path.resolve(__dirname, "../public")));

app.use("/api/books", booksApi);

app.listen(3000, () => {
  console.log("Server ready at port 3000");
});
