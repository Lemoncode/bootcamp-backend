const fs = require("fs");
const { promisify } = require("util");

// const promise = new Promise((resolve, reject) => {
//   fs.readFile("./file.txt", { encoding: "utf-8" }, (error, data) => {
//     if(error) {
//       reject(error)
//     }
//     resolve(data);
//   });
// });

const readFile = promisify(fs.readFile);

readFile("./file.txt", { encoding: "utf-8" })
  .then((data) => {
    console.log(data);
    return readFile("./file.txt", { encoding: "utf-8" });
  })
  .then((data2) => {
    console.log("Executing after");
    console.log(data2);
  })
  .catch((error) => console.error(error));

console.log("Start program");
