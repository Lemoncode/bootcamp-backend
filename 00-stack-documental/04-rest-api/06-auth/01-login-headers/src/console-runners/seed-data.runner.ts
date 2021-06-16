import { disconnect } from 'mongoose';
import { connectToDBServer } from 'core/servers';
import { envConstants } from 'core/constants';
import { bookContext } from 'dals/book/book.context';
import { db } from 'dals/mock-data';

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);
  await bookContext.insertMany(db.books);
  await disconnect();
};
