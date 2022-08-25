module.exports = {
  rootDir: '../../',
  verbose: true,
  restoreMocks: true,
  setupFiles: ['<rootDir>/config/test/env.config.js'],
  preset: '@shelf/jest-mongodb',
  watchPathIgnorePatterns: ['<rootDir>/globalConfig'],
};
