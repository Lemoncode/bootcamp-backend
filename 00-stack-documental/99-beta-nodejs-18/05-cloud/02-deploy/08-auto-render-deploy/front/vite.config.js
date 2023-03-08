import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig({
  plugins: [
    react({
      babel: {
        plugins: ['@emotion'],
      },
    }),
  ],
  resolve: {
    alias: {
      common: path.resolve(__dirname, 'src/common'),
      core: path.resolve(__dirname, 'src/core'),
      layouts: path.resolve(__dirname, 'src/layouts'),
      pods: path.resolve(__dirname, 'src/pods'),
      scenes: path.resolve(__dirname, 'src/scenes'),
      'common-app': path.resolve(__dirname, 'src/common-app'),
    },
  },
  server: {
    proxy: {
      '/api': 'http://localhost:3000',
      '/images': {
        target: 'http://localhost:3000',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/images/, ''),
      },
    },
  },
});
