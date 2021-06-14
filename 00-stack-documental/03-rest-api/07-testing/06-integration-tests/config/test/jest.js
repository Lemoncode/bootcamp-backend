const { defaults: tsPreset } = require('ts-jest/presets');

module.exports = {
  rootDir: '../../',
  preset: '@shelf/jest-mongodb',
  transform: {
    ...tsPreset.transform,
  },
  restoreMocks: true,
  moduleDirectories: ['<rootDir>/src', 'node_modules'],
  setupFiles: ['<rootDir>/config/test/env.config.js'],
  watchPathIgnorePatterns: ['<rootDir>/globalConfig'],
};
