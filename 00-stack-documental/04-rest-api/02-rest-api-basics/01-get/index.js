const http = require('http');
const { getBookList, getBook } = require('./mock-db');

const handleRequest = (req, res) => {
  const { url, method } = req;
  if (url === '/api/books' && method === 'GET') {
    res.setHeader('Content-Type', 'application/json');
    res.statusCode = 200;
    res.write(JSON.stringify(getBookList()));
    res.end();
  } else if (/\/api\/books\/\d$/.test(url) && method === 'GET') {
    const [, bookId] = url.match(/\/api\/books\/(\d)$/);
    res.setHeader('Content-Type', 'application/json');
    res.statusCode = 200;
    const book = getBook(Number(bookId));
    res.write(JSON.stringify(book));
    res.end();
  } else {
    res.write('My awesome books portal');
    res.end();
  }
};

const server = http.createServer(handleRequest);

server.listen(3000);
