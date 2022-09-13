module.exports = {
  mongodbMemoryServerOptions: {
    binary: {
      version: '5.0.9',
      skipMD5: true,
    },
    instance: {
      dbName: 'test-book-store',
      port: 27017,
    },
    autoStart: false,
  },
};
