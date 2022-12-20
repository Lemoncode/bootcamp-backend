import { Book } from "./book/index.js";

export interface DB {
  books: Book[];
}

export const db: DB = {
  books: [
    {
      id: "1",
      title: "Choque de reyes",
      releaseDate: new Date("1998-11-16"),
      author: "George R. R. Martin",
    },
    {
      id: "2",
      title: "Harry Potter y el prisionero de Azkaban",
      releaseDate: new Date("1999-07-21"),
      author: "J. K. Rowling",
    },
    {
      id: "3",
      title: "The Witcher - The Last Wish",
      releaseDate: new Date("1993-11-02"),
      author: "Andrzej Sapkowski",
    },
    {
      id: "4",
      title: "El Hobbit",
      releaseDate: new Date("1937-09-21"),
      author: "J. R. R. Tolkien",
    },
    {
      id: "5",
      title: "Assassin's Quest",
      releaseDate: new Date("1997-03-03"),
      author: "Robin Hobb",
    },
    {
      id: "6",
      title: "Homeland",
      releaseDate: new Date("1990-09-19"),
      author: "R. A. Salvatore",
    },
    {
      id: "7",
      title: "American Gods",
      releaseDate: new Date("2001-06-19"),
      author: "Neil Gaiman",
    },
  ],
};
