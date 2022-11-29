import { generateSalt, hashPassword } from 'common/helpers';
import { connectToDBServer, disconnectFromDBServer } from 'core/servers';
import { envConstants } from 'core/constants';
import { getBookContext } from 'dals/book/book.context';
import { getUserContext } from 'dals/user/user.context';
import { db } from 'dals/mock-data';

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);
  for (const user of db.users) {
    const salt = await generateSalt();
    const hashedPassword = await hashPassword(user.password, salt);

    await getUserContext().insertOne({
      ...user,
      password: hashedPassword,
      salt,
    });
  }
  await getBookContext().insertMany(db.books);
  await disconnectFromDBServer();
};
