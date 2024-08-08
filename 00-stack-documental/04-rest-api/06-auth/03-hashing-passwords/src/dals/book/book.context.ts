import { dbServer } from '#core/servers/index.js';
import { Book } from './book.model.js';

export const getBookContext = () => dbServer.db?.collection<Book>('books');
