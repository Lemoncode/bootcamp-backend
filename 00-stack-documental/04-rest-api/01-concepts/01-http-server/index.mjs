import http from "http";
import fs from "fs";

const handleRequest = (req, res) => {
  if (req.url === "/") {
    res.writeHead(200, { "Content-Type": "text/html" });
    fs.createReadStream("index.html").pipe(res);
  } else if (req.url.match(".png$")) {
    res.writeHead(200, { "Content-Type": "image/png" });
    fs.createReadStream("logo.png").pipe(res);
  } else {
    res.write("OK");
    res.end();
  }
};

const server = http.createServer(handleRequest);

server.listen(3000);
