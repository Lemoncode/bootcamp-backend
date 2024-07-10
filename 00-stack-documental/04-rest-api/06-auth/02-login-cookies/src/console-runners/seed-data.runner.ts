import { getBookContext } from '#dals/book/book.context.js';
import { db } from '#dals/mock-data.js';

export const run = async () => {
  await getBookContext().insertMany(db.books);
};
