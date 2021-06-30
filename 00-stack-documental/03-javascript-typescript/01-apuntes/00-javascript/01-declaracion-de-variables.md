# Declaración de variables

## Introducción

Una variable es un mecanismo para referenciar un valor. La sintáxis para declarar una variable es:

```
// Variables locales:
<palabra clave> <nombre de variable>

// Variables locales con valor
<palabra clave> <nombre de variable> = <valor>

// Variables globales:
<nombre variable> = <valor>
```

### let

La palabra clave `let` seguida de un nombre se utiliza para declarar una variable. Tiene ámbito de bloque, es decir, no son accesibles desde ámbitos externos.

```js
// Declaración de una variable
let name;

// Declaración de varias variables en una línea
let name, surname;
```

Se pueden asignar valores de una variable declarada con `let` en su declaración.

<!-- prettier-ignore -->
```js
// Declaración de una variable y asignación de valor
let name = "Adam";

// Declaración y asignación de varias variables
let age = 24, isVIP = true;

// No importan los saltos de línea o indentación al declarar varias variables
let a = 10,
  b = 20;
```

Una variable declarada con `let` **no puede ser redeclarada** en el mismo ámbito.

```js
let name = "Adam";
let name = "Eva";
//  ^^^^ Uncaught SyntaxError: Identifier 'name' has already been declared
```

Sin embargo podemos reasignar su valor. Una variable declarada con `let` es mutable.

```js
let name = "Adam";
name = "Eva";
```

Se pueden declarar variables con el mismo nombre siempre que estén en diferentes ámbitos.

```js
let name = "Adam";
function main() {
  let name = "Eva";
  console.log(name); // "Eva"
}
console.log(name); // "Adam"
```

### const

Cuando declaramos una variable con `const` tenemos obligatoriamente que asignar un valor. De otro modo obtendremos un error.

```js
const name;
//    ^^^^ Uncaught SyntaxError: Missing initializer in const declaration

const name = "Adam";
```

Una vez es declarada no puede ser reasignada, a diferencia de `let`.

```js
const name = "Adam";
name = "Eva";
//   ^ Uncaught TypeError: Assignment to constant variable.
```

Al igual que `let` las variables declaradas con `const` tienen ámbito de bloque y no pueden ser redeclaradas en el mismo ámbito.

```js
const name = "Adam";
const name = "Eva";
//    ^^^^ Uncaught SyntaxError: Identifier 'foo' has already been declared
```

Sin embargo, esto no ocurre si son declaradas en diferentes ámbitos:

```js
const name = "Adam";
function main() {
  const name = "Eva";
  console.log(name); // "Eva"
}
console.log(name); // "Adam"
```

Es importante tener en cuenta que `const` deniega la reasignación de la variable, pero en estructuras de datos no primitivas no significa que no podamos mutar sus propiedades.

```js
const obj = {};
obj.hello = "world"; // Es válido mutar propiedades de objeto, pero no reasignar "obj".
console.log(obj.hello); // "world"
```
