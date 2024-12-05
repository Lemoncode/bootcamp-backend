import { existsSync } from 'node:fs';
import { copyFile, mkdir } from 'node:fs/promises';

const ENV_EXAMPLE = './.env.example';
const ENV = './.env';

if (!existsSync(ENV)) {
  await copyFile(ENV_EXAMPLE, ENV);
}

const MONGO_VOLUMEN = '../mongo-data';
if (!existsSync(MONGO_VOLUMEN)) {
  await mkdir(MONGO_VOLUMEN);
}
