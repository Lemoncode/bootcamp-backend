import { ObjectId } from 'mongodb';
import { Book } from './book/index.js';
import { User } from './user/index.js';

export interface DB {
  users: User[];
  books: Book[];
}

export const db: DB = {
  users: [
    {
      _id: new ObjectId(),
      email: 'admin@email.com',
      password: 'test',
      salt: '',
      role: 'admin',
      avatar: 'admin-avatar-in-s3.png',
    },
    {
      _id: new ObjectId(),
      email: 'user@email.com',
      password: 'test',
      salt: '',
      role: 'standard-user',
      avatar: 'user-avatar-in-s3.png',
    },
  ],
  books: [
    {
      _id: new ObjectId(),
      title: 'Choque de reyes',
      releaseDate: new Date('1998-11-16'),
      author: 'George R. R. Martin',
      price: 11,
    },
    {
      _id: new ObjectId(),
      title: 'Harry Potter y el prisionero de Azkaban',
      releaseDate: new Date('1999-07-21'),
      author: 'J. K. Rowling',
      price: 12,
    },
    {
      _id: new ObjectId(),
      title: 'The Witcher - The Last Wish',
      releaseDate: new Date('1993-11-02'),
      author: 'Andrzej Sapkowski',
      price: 13,
    },
    {
      _id: new ObjectId(),
      title: 'El Hobbit',
      releaseDate: new Date('1937-09-21'),
      author: 'J. R. R. Tolkien',
      price: 14,
    },
    {
      _id: new ObjectId(),
      title: "Assassin's Quest",
      releaseDate: new Date('1997-03-03'),
      author: 'Robin Hobb',
      price: 15,
    },
    {
      _id: new ObjectId(),
      title: 'Homeland',
      releaseDate: new Date('1990-09-19'),
      author: 'R. A. Salvatore',
      price: 16,
    },
    {
      _id: new ObjectId(),
      title: 'American Gods',
      releaseDate: new Date('2001-06-19'),
      author: 'Neil Gaiman',
      price: 17,
    },
  ],
};
