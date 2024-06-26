import { MongoClient, Db } from 'mongodb';

let client: MongoClient;

const connect = async (connectionURI: string) => {
  client = new MongoClient(connectionURI);
  await client.connect();
  dbServer.db = client.db();
};

const disconnect = async () => {
  await client.close();
};

interface DBServer {
  connect: (connectionURI: string) => Promise<void>;
  disconnect: () => void;
  db: Db;
}

export let dbServer: DBServer = {
  connect,
  disconnect,
  db: undefined,
};
