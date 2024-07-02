import prompts from 'prompts';
import { runCommand } from './console-runners.helpers.js';

const SEED_DATA_ROOT_PATH = '/opt';
const SEED_DATA_FOLDER_NAME = 'app';

export const run = async () => {
  const { seedDataPath, containerName, dbName } = await prompts([
    {
      name: 'seedDataPath',
      type: 'text',
      message: 'Seed data path (in your file system):',
    },
    {
      name: 'containerName',
      initial: 'mflix-db',
      type: 'text',
      message: 'Docker container name:',
    },
    {
      name: 'dbName',
      initial: 'mflix',
      type: 'text',
      message: 'Database name:',
    },
  ]);

  const copySeedDataCommand = `docker cp "${seedDataPath}" ${containerName}:${SEED_DATA_ROOT_PATH}/${SEED_DATA_FOLDER_NAME}`;
  const restoreBackupCommand = `docker exec ${containerName} mongorestore --nsFrom="${SEED_DATA_FOLDER_NAME}.*" --nsTo="${dbName}.*" ${SEED_DATA_ROOT_PATH}`;

  await runCommand(copySeedDataCommand);
  await runCommand(restoreBackupCommand);
};
