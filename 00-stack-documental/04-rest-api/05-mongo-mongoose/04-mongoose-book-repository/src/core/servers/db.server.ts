import mongoose from 'mongoose';

const connect = async (connectionURL: string) => {
  await mongoose.connect(connectionURL);
};

interface DBServer {
  connect: (connectionURL: string) => Promise<void>;
}

export let dbServer: DBServer = {
  connect,
};
