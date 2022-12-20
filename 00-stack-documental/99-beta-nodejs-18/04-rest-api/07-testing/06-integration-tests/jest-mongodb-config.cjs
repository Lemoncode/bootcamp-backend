module.exports = {
  mongodbMemoryServerOptions: {
    binary: {
      version: '6.0.3',
      skipMD5: true,
    },
    instance: {
      dbName: 'test-book-store',
    },
    autoStart: false,
  },
};
