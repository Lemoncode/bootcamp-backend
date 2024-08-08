import fs from "node:fs";

fs.readFile("./file.txt", { encoding: "utf-8" }, (error, data) => {
  if (error) {
    console.error(error);
  } else {
    console.log(data);
  }
});

console.log("Start program");
