import { MongoClient, Db } from 'mongodb';

let client: MongoClient;

const connect = async (connectionURL: string) => {
  client = new MongoClient(connectionURL);
  await client.connect();
  dbServer.db = client.db();
};

const disconnect = async () => {
  await client.close();
};

interface DBServer {
  connect: (connectionURL: string) => Promise<void>;
  disconnect: () => Promise<void>;
  db: Db;
}

export let dbServer: DBServer = {
  connect,
  disconnect,
  db: undefined,
};
