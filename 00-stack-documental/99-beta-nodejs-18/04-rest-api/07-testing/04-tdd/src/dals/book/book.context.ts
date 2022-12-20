import { db } from '#core/servers/index.js';
import { Book } from './book.model.js';

export const getBookContext = () => db?.collection<Book>('books');
