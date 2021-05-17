const fs = require("fs");
const { promisify } = require("util");

const readFile = promisify(fs.readFile);

const run = async () => {
  try {
    const data = await readFile("./file.txt", { encoding: "utf-8" });
    console.log(data);
    console.log("Executing after");
    const data2 = await readFile("./file.txt", { encoding: "utf-8" });
    console.log(data2);
  } catch (error) {
    console.error(error);
  }
};

run();
console.log("Start program");
