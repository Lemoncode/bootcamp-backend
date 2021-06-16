import { prompt } from 'inquirer';
import { runCommand } from './console-runners.helpers';

const seedDataContainerPath = '/opt/app';

export const run = async () => {
  const { seedDataPath, containerName, dbName } = await prompt([
    {
      name: 'seedDataPath',
      type: 'input',
      message: 'Seed data path (in your file system):',
    },
    {
      name: 'containerName',
      type: 'input',
      message: 'Docker container name:',
    },
    {
      name: 'dbName',
      type: 'input',
      message: 'Database name:',
    },
  ]);

  const copySeedDataCommand = `docker cp "${seedDataPath}" ${containerName}:${seedDataContainerPath}`;
  const restoreBackupCommand = `docker exec ${containerName} mongorestore --db ${dbName} ${seedDataContainerPath}`;

  await runCommand(copySeedDataCommand);
  await runCommand(restoreBackupCommand);
};
