import { connectToDBServer, disconnectFromDBServer } from 'core/servers';
import { envConstants } from 'core/constants';
import { getBookContext } from 'dals/book/book.context';
import { db } from 'dals/mock-data';

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);
  await getBookContext().insertMany(db.books);
  await disconnectFromDBServer();
};
