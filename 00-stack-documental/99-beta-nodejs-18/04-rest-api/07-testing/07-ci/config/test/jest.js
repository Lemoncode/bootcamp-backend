export default {
  rootDir: '../../',
  verbose: true,
  restoreMocks: true,
  setupFiles: ['<rootDir>/config/test/env.config.js'],
  preset: '@shelf/jest-mongodb',
  watchPathIgnorePatterns: ['<rootDir>/globalConfig'],
  transform: {
    '^.+\\.tsx?$': [
      'ts-jest',
      {
        useESM: true,
      },
    ],
  },
  extensionsToTreatAsEsm: ['.ts'],
  moduleNameMapper: {
    '^(\\.{1,2}/.*)\\.js$': '$1',
    '#(.*)\\.js$': '<rootDir>/src/$1',
  },
};
