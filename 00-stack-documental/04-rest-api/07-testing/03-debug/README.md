# 03 Debug

In this example we are going to configure VS Code for debugging Jest specs.

We will start from `02-basic-concepts`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
npm install
```

## Debugging Jest

Jest is running over node, so we could use VS Code for debugging jest specs:

### Using JavaScript Debug Terminal

Since `jest` is a nodejs process, we could use the integraded `JavaScript Debug Terminal` provided by VS Code.

> More info [here](https://www.lemoncode.tv/curso/vs-code-js-debugging/leccion/javascript-debug-terminal)

We could run all specs as `single run` in this terminal and adding some breakpoints:

```bash
npm test

```

We could run all specs as `watch run` in this terminal and adding some breakpoints:

```bash
npm run test:watch

```

We could run specs related to specific file or files:

_./src/second.spec.ts_

```typescript
describe('second specs', () => {
  it('should return true', () => {
    expect(true).toBeTruthy();
  });
});

```

```bash
npm run test:watch second
npm run test:watch second.spec
npm run test:watch spec

```

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).
