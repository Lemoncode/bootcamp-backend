import mongoose from 'mongoose';

const connect = async (connectionURI: string) => {
  await mongoose.connect(connectionURI);
};

interface DBServer {
  connect: (connectionURI: string) => Promise<void>;
}

export let dbServer: DBServer = {
  connect,
};
