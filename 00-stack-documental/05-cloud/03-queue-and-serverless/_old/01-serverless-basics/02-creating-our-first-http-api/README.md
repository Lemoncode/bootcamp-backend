# Creating Our First HTTP API

We have to provide permissions to create a new user in DynamoDB

```diff
service: gpc-api

frameworkVersion: '2'


provider:
  name: aws
  runtime: nodejs12.x
  lambdaHashingVersion: 20201221
+ iamRoleStatements:
+   - Effect: Allow
+     Action:
+       - dynamodb:PutItem
+     Resource: "arn:aws:dynamodb:eu-west-2:*:table/users"
+
functions:
  hello:
    handler: handler.hello

```

Create the configuration for the HTTP API that we want to create.

```yaml
service: gpc-api

frameworkVersion: '2'


provider:
  name: aws
  runtime: nodejs12.x
  lambdaHashingVersion: 20201221
  iamRoleStatements:
    - Effect: Allow
      Action:
        - dynamodb:PutItem
      Resource: "arn:aws:dynamodb:eu-west-2:*:table/users"
# diff
functions:
  createUser:
    handler: handler.handler
    events:
      - http:
          path: /user
          method: POST
          corts: true
# diff
```

Update `handler.js`

```diff
'use strict';

-module.exports.hello = async (event) => {
+module.exports.handler = async (event) => {
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

In  order to test locally and don't have to deploy the lambda function we are going ti use an additional package: [serverless-offline](https://github.com/dherault/serverless-offline)

```bash
$ npm init -y
```

```bash
$ npm install serverless-offline --save-dev
```

We have to update `serverless.yml`

```diff
service: gpc-api

frameworkVersion: '2'


provider:
  name: aws
  runtime: nodejs12.x
+ region: eu-west-2
  lambdaHashingVersion: 20201221
  iamRoleStatements:
    - Effect: Allow
      Action:
        - dynamodb:PutItem
        # - dynamodb:Query
      Resource: "arn:aws:dynamodb:eu-west-2:*:table/users"

functions:
  createUser:
    handler: handler.handler
    events:
      - http:
          path: /user
          method: POST
          corts: true
+
+plugins:
+ - serverless-offline
+
```

And update `package.json`

```diff
{
  "name": "gpc-api",
  "version": "1.0.0",
  "description": "<!-- title: 'AWS NodeJS Example' description: 'This template demonstrates how to deploy a NodeJS function running on AWS Lambda using the traditional Serverless Framework.' layout: Doc framework: v2 platform: AWS language: nodeJS authorLink: 'https://github.com/serverless' authorName: 'Serverless, inc.' authorAvatar: 'https://avatars1.githubusercontent.com/u/13742415?s=200&v=4' -->",
  "main": "handler.js",
  "scripts": {
+   "start": "serverless offline", 
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

Now that this is up and running we can test it locally by running, we can use this [gist](https://gist.github.com/subfuzion/08c5d85437d5d4f00e58) 

```bash
$ curl -X POST http://localhost:3000/dev/user
```

## Adding a DynamoDB Table


```yaml
service: gpc-api

frameworkVersion: '2'

provider:
  name: aws
  runtime: nodejs12.x
  region: eu-west-2
  lambdaHashingVersion: 20201221
  iamRoleStatements:
    - Effect: Allow
      Action:
        - dynamodb:PutItem
        # - dynamodb:Query
      Resource: "arn:aws:dynamodb:eu-west-2:*:table/users"

functions:
  createUser:
    handler: handler.handler
    events:
      - http:
          path: /user
          method: POST
          cors: true
# diff #
resources:
  Resources:
    usersTable:
      Type: AWS::DynamoDB::Table
      Properties:
        TableName: users
        AttributeDefinitions:
          - AttributeName: email
            AttributeType: S
        KeySchema:
          - AttributeName: email
            KeyType: HASH
        ProvisionedThroughput:
          ReadCpacityUnits: 1
          WriteCpacityUnits: 1
# diff #
plugins:
  - serverless-offline

```

If we run now a new deploy

```bash
$ serverless deploy
Serverless: Running "serverless" installed locally (in service node_modules)
Serverless: Deprecation warning: Starting with version 3.0.0, following property will be replaced:
              "provider.iamRoleStatements" -> "provider.iam.role.statements"
            More Info: https://www.serverless.com/framework/docs/deprecations/#PROVIDER_IAM_SETTINGS
Serverless: Packaging service...
Serverless: Excluding development dependencies...
Serverless: Uploading CloudFormation file to S3...
Serverless: Uploading artifacts...
Serverless: Uploading service gpc-api.zip file to S3 (267.66 KB)...
Serverless: Validating template...
Serverless: Updating Stack...
Serverless: Checking Stack update progress...
....................................
Serverless: Stack update finished...
Service Information
service: gpc-api
stage: dev
region: eu-west-2
stack: gpc-api-dev
resources: 13
api keys:
  None
endpoints:
  POST - https://dn6m8weaqc.execute-api.eu-west-2.amazonaws.com/dev/user
functions:
  createUser: gpc-api-dev-createUser
layers:
  None
```

Now we can verify that the new resources are created by:

```bash
$ aws dynamodb list-tables
{
    "TableNames": [
        "users"
    ]
}
```

## Adding New users

To add new users to our `DynamoDB table`, we need to install a new package:

```bash
$ npm install @aws-sdk/client-dynamodb
```

[DynamoDB Reference](https://docs.aws.amazon.com/AWSJavaScriptSDK/v3/latest/clients/client-dynamodb/index.html)

Update `handler.js` as follows:

```js
'use strict';
/*diff*/
const { DynamoDBClient, PutItemCommand } = require('@aws-sdk/client-dynamodb');

const client = new DynamoDBClient({ region: 'eu-west-2' });
/*diff*/
module.exports.handler = async (event) => {
  /*diff*/  
  try {
    const params = {
      TableName: 'users',
      Item: {
        'email': { 'S': 'jaime.salas@lemoncode.net' }
      },
    };

    const command = new PutItemCommand(params);
    await client.send(command);

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
  } catch (error) {
    console.error(error);
  }
  /*diff*/
};

```

To test it we can run

```bash
$ curl -X POST http://localhost:3000/dev/user
```

## Cleanup

To clean up resources we can use `CloudFormation` from AWS Console
