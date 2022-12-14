import { db } from 'core/servers';
import { Book } from './book.model';

export const getBookContext = () => db?.collection<Book>('books');
