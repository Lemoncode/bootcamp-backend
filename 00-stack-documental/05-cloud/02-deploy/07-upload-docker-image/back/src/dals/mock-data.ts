import { ObjectId } from 'mongodb';
import { Book } from './book';
import { User } from './user';

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
    },
    {
      _id: new ObjectId(),
      title: 'Harry Potter y el prisionero de Azkaban',
      releaseDate: new Date('1999-07-21'),
      author: 'J. K. Rowling',
    },
    {
      _id: new ObjectId(),
      title: 'The Witcher - The Last Wish',
      releaseDate: new Date('1993-11-02'),
      author: 'Andrzej Sapkowski',
    },
    {
      _id: new ObjectId(),
      title: 'El Hobbit',
      releaseDate: new Date('1937-09-21'),
      author: 'J. R. R. Tolkien',
    },
    {
      _id: new ObjectId(),
      title: "Assassin's Quest",
      releaseDate: new Date('1997-03-03'),
      author: 'Robin Hobb',
    },
    {
      _id: new ObjectId(),
      title: 'Homeland',
      releaseDate: new Date('1990-09-19'),
      author: 'R. A. Salvatore',
    },
    {
      _id: new ObjectId(),
      title: 'American Gods',
      releaseDate: new Date('2001-06-19'),
      author: 'Neil Gaiman',
    },
  ],
};
