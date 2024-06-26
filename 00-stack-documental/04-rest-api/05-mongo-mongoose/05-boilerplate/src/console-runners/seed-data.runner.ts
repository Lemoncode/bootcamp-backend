import prompts from 'prompts';
import { runCommand } from './console-runners.helpers.js';

const seedDataContainerPath = '/opt/app';

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

  const copySeedDataCommand = `docker cp "${seedDataPath}" ${containerName}:${seedDataContainerPath}`;
  const restoreBackupCommand = `docker exec ${containerName} mongorestore --db ${dbName} ${seedDataContainerPath}`;

  await runCommand(copySeedDataCommand);
  await runCommand(restoreBackupCommand);
};
