import { disconnect } from 'mongoose';
import { envConstants } from 'core/constants';
import { connectToDBServer } from 'core/servers';

const runQueries = async () => {};

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);
  await runQueries();
  await disconnect();
};
