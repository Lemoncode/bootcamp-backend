import { prompt } from 'inquirer';

(async () => {
  let exit = false;
  while (!exit) {
    const answer = await prompt({
      name: 'consoleRunner',
      type: 'list',
      message: 'Which console-runner do you want to run?',
      choices: ['seed-data', 'exit'],
    });

    if (answer.consoleRunner !== 'exit') {
      const { run } = require(`./${answer.consoleRunner}.runner`);
      await run();
    } else {
      exit = true;
    }
  }
})();
