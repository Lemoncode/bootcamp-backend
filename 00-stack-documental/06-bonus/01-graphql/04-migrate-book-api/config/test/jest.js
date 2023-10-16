export default {
  rootDir: '../../',
  verbose: true,
  restoreMocks: true,
  setupFiles: ['<rootDir>/config/test/env.config.js'],
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
