import { MongoClient, Db } from "mongodb";

let dbInstance: Db;

export const connectToDB = async (connectionURI: string) => {
  const client = new MongoClient(connectionURI);
  await client.connect();

  dbInstance = client.db();
};

export const getDBInstance = (): Db => dbInstance;
