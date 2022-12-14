import {
  connectToDBServer,
  disconnectFromDBServer,
} from '#core/servers/index.js';
import { envConstants } from '#core/constants/index.js';
import { getBookContext } from '#dals/book/book.context.js';
import { db } from '#dals/mock-data.js';

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);
  await getBookContext().insertMany(db.books);
  await disconnectFromDBServer();
};
