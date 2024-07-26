# 07 CI

In this example we will configure continuos integration and run tests.

We will start from `06-integration-tests`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install
```

We will configure [Github actions](https://github.com/features/actions) to run all tests in this app. Since Github has [free private/public repositories](https://github.com/pricing) we only need to create a github repository:

```bash
git init
git remote add origin git@...git
git add .
git commit -m "add project with tests"
git push -u origin main
```

Create new branch on repository `feature/add-ci-file` and add ci config [Github workflow](https://help.github.com/en/actions/configuring-and-managing-workflows/configuring-a-workflow):

_./.github/workflows/ci.yml_

```yml
name: CI workflow

on: pull_request

jobs:
  ci:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
      - name: Install
        run: npm ci
      - name: Tests
        run: npm test

```

Commit, push:

```bash
git add .
git commit -m "add ci file"
git push -u origin feature/add-ci-file
```

Create a pull request.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
