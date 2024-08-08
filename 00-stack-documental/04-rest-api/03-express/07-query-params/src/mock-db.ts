interface Book {
  id: number;
  title: string;
  releaseDate: string;
  author: string;
}

let mockBookList: Book[] = [
  {
    id: 1,
    title: "Choque de reyes",
    releaseDate: "16/11/1998",
    author: "George R. R. Martin",
  },
  {
    id: 2,
    title: "Harry Potter y el prisionero de Azkaban",
    releaseDate: "21/07/1999",
    author: "J. K. Rowling",
  },
  {
    id: 3,
    title: "The Witcher - The Last Wish",
    releaseDate: "02/11/1993",
    author: "Andrzej Sapkowski",
  },
  {
    id: 4,
    title: "El Hobbit",
    releaseDate: "21/09/1937",
    author: "J. R. R. Tolkien",
  },
  {
    id: 5,
    title: "Assassin's Quest",
    releaseDate: "03/03/1997",
    author: "Robin Hobb",
  },
  {
    id: 6,
    title: "Homeland",
    releaseDate: "19/09/1990",
    author: "R. A. Salvatore",
  },
  {
    id: 7,
    title: "American Gods",
    releaseDate: "19/06/2001",
    author: "Neil Gaiman",
  },
];

export const getBookList = async () => {
  return mockBookList;
};

export const getBook = async (id: number) => {
  return mockBookList.find((book) => book.id === id);
};

export const insertBook = async (book: Book) => {
  const id = mockBookList.length + 1;
  const newBook = {
    ...book,
    id,
  };

  mockBookList = [...mockBookList, newBook];
  return newBook;
};

export const updateBook = async (id: number, updatedBook: Book) => {
  mockBookList = mockBookList.map((book) =>
    book.id === id
      ? {
          ...book,
          ...updatedBook,
          id,
        }
      : book
  );
};

export const deleteBook = async (id: number) => {
  mockBookList = mockBookList.filter((book) => book.id !== id);
  return true;
};
