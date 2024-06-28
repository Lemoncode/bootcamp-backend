import { MongoClient, Db } from 'mongodb';

let client: MongoClient;

const connect = async (connectionURI: string) => {
  client = new MongoClient(connectionURI);
  await client.connect();
  dbServer.db = client.db();
};

interface DBServer {
  connect: (connectionURI: string) => Promise<void>;
  db: Db;
}

export let dbServer: DBServer = {
  connect,
  db: undefined,
};
