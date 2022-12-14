import { connect } from 'mongoose';

export const connectToDBServer = async (connectionURI: string) => {
  await connect(connectionURI);
};
