import prompts from 'prompts';
import { ENV } from '#core/constants/index.js';
import { dbServer } from '#core/servers/index.js';

let exit = false;

const { connectionString } = await prompts({
  name: 'connectionString',
  type: 'text',
  message: 'Connection string (Press enter to use default): ',
});
await dbServer.connect(connectionString ?? ENV.MONGODB_URI);

while (!exit) {
  const { consoleRunner } = await prompts({
    name: 'consoleRunner',
    type: 'select',
    message: 'Which console-runner do you want to run?',
    choices: ['queries', 'seed-data', 'exit'].map((option) => ({
      title: option,
      value: option,
    })),
  });

  if (consoleRunner !== 'exit') {
    const { run } = await import(`./${consoleRunner}.runner.js`);
    await run();
  } else {
    exit = true;
    await dbServer.disconnect();
  }
}
