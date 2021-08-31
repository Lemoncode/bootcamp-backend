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

When ever a new sold request arrives to our lambda function we will publish a new message into the queue. We need to install a new package to deal with our queue, `cd` into `02-putting-all-together/serverless-solutions/lc-sold-notifier`

```bash
$ npm i amqplib -S
```

```bash
$ npm i @types/amqplib -D
```


Let's create a new publisher, but before, let's create some utils, to make our code more readable, `02-putting-all-together/serverless-solutions/lc-sold-notifier/amqp-utils.js`

```js
const amqp = require('amqplib');

const url = 'amqp://guest:guest@localhost:5672';

let connection;

module.exports.openChannel = async () => {
    connection = await amqp.connect(url);
    return connection.createChannel();
};

module.exports.close = async () => {
    await connection.close();
};

```

Now we cna create `02-putting-all-together/serverless-solutions/lc-sold-notifier/publisher.js`

```js
const { openChannel, close } = require('./amqp-utils');

module.exports.Publisher = class {
    constructor(exchange, type) {
        this.channel = null;
        this.exchange = exchange;
        this.type = type;
    }

    async startConnection() {
        try {
            this.channel = await openChannel();
        } catch (error) {
            console.log(error);
            throw error;
        }
    }

    async assertExchange(options) {
        if (!this.channel) {
            throw 'Not channel initialised, you must open a channel before use this method.';
        }

        try {
            await this.channel.assertExchange(this.exchange, this.type, options);
        } catch (error) {
            console.log(error);
            throw error;
        }
    }

    publish(msg) {
        try {
            this.channel.publish(this.exchange, '', Buffer.from(msg));
        } catch (error) {
            console.log(error);
            throw error;
        }
    }

    async close() {
        await close();
    }
};

```

Now we have to update `02-putting-all-together/serverless-solutions/lc-sold-notifier/handler.js`, to publish the message.

```js
'use strict';

const Publisher = require('./publisher');

module.exports.sold = async (event) => {
  const { body } = event;
  const publisher = new Publisher('sales', 'fanout');
  await publisher.startConnection();
  await publisher.assertExchange();
  publisher.publish(body);
  
  return {
    statusCode: 200,
    body: JSON.stringify(
      {
        message: 'message send to queue',
      },
      null,
      2
    ),
  };
};

```

## Consume the message

Now let's create a new project to host the message consumers. At the same level as `serverless-solutions`, run:

```bash
$ mkdir subscriber; cd subscriber
```

```bash
$ npm init -y 
```

```bash
$ npm install amqplib -S
```

```bash
$ npm install @types/amqplib -D
```

Just for simplicity of demo, we're going to create inside `susbscriber` directory `amqp-utils.js`. This duplicity is bad, really bad, there are better approaches to this, such as reference a lib inside of the same diretory tree, and the best one create an external library that we can import from a package manager such as `npm registry`. As bonus track, we will include how to import libraries from the sametree directory.

```js
const amqp = require('amqplib');

const url = 'amqp://guest:guest@localhost:5672';

let connection;

module.exports.openChannel = async () => {
    connection = await amqp.connect(url);
    return connection.createChannel();
};

module.exports.close = async () => {
    await connection.close();
};

```

Now let's create `02-putting-all-together/subscriber/index.js`

```js
const { openChannel } = require('./amqp-utils');

const run = async () => { 
    try {
        const channel = await openChannel();
        const exchange = 'sales';
        await channel.assertExchange(exchange, 'fanout', { durable: false});
        const { queue } = await channel.assertQueue('');
        await channel.bindQueue(queue, exchange, '');
        channel.consume(
            queue,
            (msg) => {
                const { content } = msg;
                console.log(content.toString());
                channel.ack(msg);
            }
        );
    } catch (error) {
        
    }
};

run();

```

Update `package.json`

```diff
{
  "name": "subscriber",
  "version": "1.0.0",
  "description": "",
  "main": "index.js",
  "scripts": {
+   "start": "node .",
    "test": "echo \"Error: no test specified\" && exit 1"
  },
  "keywords": [],
  "author": "",
  "license": "ISC",
  "dependencies": {
    "amqplib": "^0.8.0"
  },
  "devDependencies": {
    "@types/amqplib": "^0.8.1"
  }
}

```

## Running the solution


```bash
$ ./02-putting-all-together/start-up-message-broker.sh
$ cd ./02-putting-all-together/serverless-solutions/lc-sold-notifier
$ npm start
```

From a new terminal

```bash
$ cd ./02-putting-all-together/subscriber
$ npm start
```

Open a new terminal to send a new request:

```bash
$ curl -d '{"bookId":"fagd-3452", "value":"9,99$"}' -H "Content-Type: application/json" -X POST http://localhost:3000/dev/sold
```

## Clenup

```bash
$ docker stop rabbit
```
