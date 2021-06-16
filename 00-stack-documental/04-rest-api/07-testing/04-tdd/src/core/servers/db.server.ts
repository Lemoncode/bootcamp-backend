import { connect, set } from 'mongoose';
import { envConstants } from 'core/constants';

set('debug', !envConstants.isProduction);

export const connectToDBServer = async (connectionURI: string) => {
  await connect(connectionURI, {
    useNewUrlParser: true, // https://mongoosejs.com/docs/deprecations.html#the-usenewurlparser-option
    useUnifiedTopology: true, // https://mongoosejs.com/docs/deprecations.html#useunifiedtopology
    useFindAndModify: false, // https://mongoosejs.com/docs/deprecations.html#findandmodify
  });
};
