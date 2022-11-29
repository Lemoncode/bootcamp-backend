import fs from "fs";
import { promisify } from "util";

const readFile = promisify(fs.readFile);

try {
  const data = await readFile("./file.txt", { encoding: "utf-8" });
  console.log(data);
  console.log("Executing after");
  const data2 = await readFile("./file.txt", { encoding: "utf-8" });
  console.log(data2);
} catch (error) {
  console.error(error);
}

console.log("Start program");
