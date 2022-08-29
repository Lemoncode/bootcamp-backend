## Reference

[serverless get started](https://www.serverless.com/framework/docs/getting-started/)

## Config AWS CLI

Create a new user with programatic access and enough privileges.

```bash
$ aws configure
```


## Install via NPM

```bash
$ npm install -g serverless
```

## Start a new serverless service

```bash
$ serverless 
What do you want to make? AWS - Node.js - Starter
 What do you want to call this project? gpc-api

Downloading "aws-node" template...

Project successfully created in gpc-api folder

You are not logged in or you do not have a Serverless account.

 Do you want to login/register to Serverless Dashboard? No

 Do you want to deploy your project? No

Your project is ready for deployment and available in ./gpc-api

  Run serverless deploy in the project directory
    Deploy your newly created service

  Run serverless info in the project directory after deployment
    View your endpoints and services

  Run serverless invoke and serverless logs in the project directory after deployment
    Invoke your functions directly and view the logs

  Run serverless in the project directory
    Add metrics, alerts, and a log explorer, by enabling the dashboard functionality
```

If we want to deploy our code, we can cd into `gpc-api` and run:

```bash
$ serverless deploy
```

> NOTE: If we want to use deploy command we need an AWS account completely set on the container. Follow the [link](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-quickstart.html) to find the AWS CLI basic configuration documentation. 

What this is going to do behind the scenes is use `CloudFormation` to deploy our code, the way that we can specify the configuration for that deployment come with `serverless.yml`

```yml
service: gpc-api

frameworkVersion: '2'


provider:
  name: aws
  runtime: nodejs12.x
  lambdaHashingVersion: 20201221

functions:
  hello:
    handler: handler.hello

```
