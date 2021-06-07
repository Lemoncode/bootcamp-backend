import { bookContext } from 'dals/book/book.context';
import { db } from 'dals/mock-data';

export const run = async () => {
  await bookContext.bulkWrite(db.books);
};
