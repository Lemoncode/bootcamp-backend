import http from "http";
import { getBookList, getBook, insertBook } from "./mock-db.mjs";

const handleRequest = (req, res) => {
  const { url, method } = req;
  if (url === "/api/books") {
    if (method === "GET") {
      res.setHeader("Content-Type", "application/json");
      res.write(JSON.stringify(getBookList()));
      res.statusCode = 200;
      res.end();
    } else if (method === "POST") {
      let data = [];
      req.on("data", (chunk) => {
        console.log({ chunk });
        data += chunk;
      });

      req.on("end", () => {
        const book = JSON.parse(data.toString("utf8"));
        const newBook = insertBook(book);
        res.setHeader("Content-Type", "application/json");
        res.statusCode = 201;
        res.write(JSON.stringify(newBook));
        res.end();
      });
    }
  } else if (/\/api\/books\/\d$/.test(url) && method === "GET") {
    const [, bookId] = url.match(/\/api\/books\/(\d)$/);
    res.setHeader("Content-Type", "application/json");
    res.statusCode = 200;
    const book = getBook(Number(bookId));
    res.write(JSON.stringify(book));
    res.end();
  } else {
    res.write("My awesome books portal");
    res.end();
  }
};

const server = http.createServer(handleRequest);

server.listen(3000);
