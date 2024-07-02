import { MongoClient, Db } from 'mongodb';

let client: MongoClient;

const connect = async (connectionURL: string) => {
  client = new MongoClient(connectionURL);
  await client.connect();
  dbServer.db = client.db();
};

interface DBServer {
  connect: (connectionURL: string) => Promise<void>;
  db: Db;
}

export let dbServer: DBServer = {
  connect,
  db: undefined,
};
