import { config } from 'dotenv';

export function setup() {
  config({
    path: './.env.test',
  });
}
