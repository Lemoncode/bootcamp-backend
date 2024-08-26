import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    globals: true,
    restoreMocks: true,
    globalSetup: [
      './config/test/env.config.ts',
      './config/test/db-server.config.ts',
    ],
  },
});
