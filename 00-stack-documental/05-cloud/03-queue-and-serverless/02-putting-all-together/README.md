# Putting All Together

The functionality that we're looking for is the generation of an invoice when ever a new book, from our book store has been sold. For that purpose we will create a lambda function that will listen for a HTTP event, then will send a message to a queue, a set of workers will listen to this queue and will produce the invoice as a PDF document.


## Creating the lambda function

To work with `serverless framework`, we have to install it locally. There are different ways to do this, one is using Nodejs:

```bash
$ npm i -g serverless
```

Another approach to avoid install the package globally will be using `npx` to run the binaries on `local node_modules`.

The `serverless framework` has a direct dependency with AWS (or other cloud providers such as Azure). What is going to do when the package is installed, is veryfied that we have an AWS account installed an ready for use. So as **prerequisite** we need AWS CLI installed and configured.

```bash
$ mkdir serverless-solutions; cd serverless-solutions
```

```bash
$ npm init -y
```

```bash
$ npm install serverless npx -D
```

No we can `cd` into serverless-solution directory an run the following:

```bash
$ npx serverless

 What do you want to make? AWS - Node.js - Starter
 What do you want to call this project? lc-sold-notifier

Downloading "aws-node" template...

Project successfully created in lc-sold-notifier folder

You are not logged in or you do not have a Serverless account.

 Do you want to login/register to Serverless Dashboard? No

 Do you want to deploy your project? No

Your project is ready for deployment and available in ./lc-sold-notifier

  Run serverless deploy in the project directory
    Deploy your newly created service

  Run serverless info in the project directory after deployment
    View your endpoints and services

  Run serverless invoke and serverless logs in the project directory after deployment
    Invoke your functions directly and view the logs

  Run serverless in the project directory
    Add metrics, alerts, and a log explorer, by enabling the dashboard functionality
```

Now we can `cd` into `lc-sold-notifier` and run:

```bash
$ npm init -y
```

```bash
$ npm i serverless-offline npx -D
```

Update `serverless.yml`

```yml
service: lc-sold-notifier

frameworkVersion: '2'


provider:
  name: aws
  runtime: nodejs12.x
  lambdaHashingVersion: 20201221

functions:
  hello:
    handler: handler.sold
    events:
      - http:
          path: /sold
          method: POST
          cors: true

plugins: 
  - serverless-offline

```

Update `handler.js`

```diff
'use strict';

-module.exports.hello = async (event) => {
+module.exports.sold = async (event) => {
+ const { body } = event;
+ console.log(body);
  return {
    statusCode: 200,
    body: JSON.stringify(
      {
        message: 'Go Serverless v2.0! Your function executed successfully!',
        input: event,
      },
      null,
      2
    ),
  };
};
```

Update `package.json`

```diff
{
  "name": "lc-sold-notifier",
  "version": "1.0.0",
  "description": "<!-- title: 'AWS NodeJS Example' description: 'This template demonstrates how to deploy a NodeJS function running on AWS Lambda using the traditional Serverless Framework.' layout: Doc framework: v2 platform: AWS language: nodeJS authorLink: 'https://github.com/serverless' authorName: 'Serverless, inc.' authorAvatar: 'https://avatars1.githubusercontent.com/u/13742415?s=200&v=4' -->",
  "main": "handler.js",
  "scripts": {
+   "start": "npx serverless offline",
    "test": "echo \"Error: no test specified\" && exit 1"
  },
  "keywords": [],
  "author": "",
  "license": "ISC",
  "devDependencies": {
    "serverless-offline": "^8.0.0"
  }
}

```

Open a new terminal and run `npm start`, and in another terminal run:

```bash
$ curl -d '{"key1":"value1", "key2":"value2"}' -H "Content-Type: application/json" -X POST http://localhost:3000/dev/sold
```

Now we can check the log output on lambda console and we must see:

```bash
{"key1":"value1", "key2":"value2"}
```

## Pushing a message to the queue

## Consume the message
