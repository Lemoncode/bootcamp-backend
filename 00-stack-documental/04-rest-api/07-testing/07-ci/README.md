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
git remote add origin https://github.com/...
git add .
git commit -m "add project with tests"
git push -u origin master
```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
