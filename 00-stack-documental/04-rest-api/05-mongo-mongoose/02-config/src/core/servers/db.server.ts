import { MongoClient, Db } from 'mongodb';

export let db: Db;

export const connectToDBServer = async (connectionURI: string) => {
  const client = new MongoClient(connectionURI);
  await client.connect();

  db = client.db();
};
