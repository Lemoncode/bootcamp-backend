import { MongoMemoryServer } from 'mongodb-memory-server';

export async function setup() {
  const dbServer = await MongoMemoryServer.create({
    instance: {
      port: 27017,
      dbName: 'test-book-store',
    },
  });

  return async () => {
    await dbServer.stop();
  };
}
