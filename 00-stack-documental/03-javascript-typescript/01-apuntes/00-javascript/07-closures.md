# Closures

Las funciones puede acceder a variables declaradas fuera del propio cuerpo de la función, tienen acceso a los contextos superiores:

```js
const phone = "927372731";

function printPhone() {
  console.log(phone);
}

printPhone(); // "927372731"
```

Sin embargo, las variables declaradas dentro de una función no son visibles desde fuera. Esto es así porque las funciones crean un nuevo ámbito (scope) para las variables:

```js
function printPhone() {
  const phone = "927372731";
  console.log(phone);
}
printPhone(); // "927372731"
console.log(phone); // "ReferenceError: phone is not defined"
```

Pero aún hay más. Una función dentro de otra, puede acceder a cualquier declaración hecha en el cuerpo de la función exterior .... ¡en cualquier momento! Es decir, la función interior tiene acceso al contexto léxico donde fue declarada, y ese contexto es el cuerpo de la función exterior. A esto se le conoce como **CLOSURE**:

```js
function createCounter() {
  let i = 0;
  return function () {
    // Desde aqui tenemos acceso a la variable "i"
    console.log(i++);
  };
}

const counter = createCounter();
counter(); // 0
counter(); // 1
counter(); // 2

// No tendremos acceso a i desde fuera
console.log(i); // "Uncaught ReferenceError: i is not defined"
```

Los CLOSURES son muy útiles para encapsular datos (haciéndolos privados) y métodos juntos. Es decir, privatizamos datos y ofrecemos métodos (o una interfaz completa) para manejarlos. Esto se asemeja mucho al funcionamiento de una clase.

El ejemplo anterior se podría evolucionar un poco hasta obtener:

```js
function createCounter() {
  let i = 0;
  return {
    increase: function () {
      i++;
    },
    decrease: function () {
      i--;
    },
    print: function () {
      console.log(i);
    },
  };
}

const counter = createCounter();
counter.increase();
counter.print(); // 1
counter.decrease();
counter.print(); // 0
```
