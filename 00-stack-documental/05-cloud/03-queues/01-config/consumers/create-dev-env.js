import { existsSync } from 'node:fs';
import { copyFile } from 'node:fs/promises';

const ENV_EXAMPLE = './.env.example';
const ENV = './.env';

if (!existsSync(ENV)) {
  await copyFile(ENV_EXAMPLE, ENV);
}
