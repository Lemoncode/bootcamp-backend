const http2 = require('http2');
const fs = require('fs');

const serverOptions = {
  key: fs.readFileSync('certificate/key.pem'),
  cert: fs.readFileSync('certificate/cert.pem'),
};

const handleRequest = (req, res) => {
  if (req.url === '/') {
    res.writeHead(200, { 'Content-Type': 'text/html' });
    fs.createReadStream('index.html').pipe(res);
  } else if (req.url.match('.png$')) {
    res.writeHead(200, { 'Content-Type': 'image/png' });
    fs.createReadStream('logo.png').pipe(res);
  }
};

const server = http2.createSecureServer(serverOptions, handleRequest);

server.listen(3000);
