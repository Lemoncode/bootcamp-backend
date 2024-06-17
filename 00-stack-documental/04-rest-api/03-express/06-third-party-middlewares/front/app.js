console.log('Running front app');

fetch('http://localhost:3000/api/books/2')
  .then((response) => {
    return response.json();
  })
  .then((book) => {
    console.log({ book });
  });
