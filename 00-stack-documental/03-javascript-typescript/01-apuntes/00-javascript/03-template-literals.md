# Template literals

En javascript, los strings pueden representarse utilizando comillas dobles (") o simples ('), pero además, podemos usar los backticks:

```js
const message = `Hello`;
console.log(message); // "Hello"
```

Una ventaja que ofrecen es poder crear strings multilinea sin concatenación:

```js
const riddle = `Sobre las oscuras sombras
del antiguo bastión,
el alto centinela cuadrado callado ve
correr la sangre en el noveno mes`;
console.log(riddle);
/*
"Sobre las oscuras sombras
del antiguo bastión,
el alto centinela cuadrado callado ve
correr la sangre en el noveno mes"
*/
```

> IMPORTANTE saber que si estamos trabajando con código indentado, como por ejemplo dentro de una función, los espacios se mantienen.

Pero quizá la cualidad más importante es que permiten interpolación de código, mediante _placeholders_. A esto se le conoce como _template literals_:

```js
const name = "Edward";
const message = `How are you, ${name}?`;
console.log(message); // "How are you, Edward?"
```

Ejemplo más elaborado:

<!-- prettier-ignore -->
```js
const formatMessage = (product, quantity) =>
  `You have ${quantity} ${product}${quantity === 1 ? "" : "s"} in the shopping cart`;

console.log(formatMessage("egg", 3)); // "You have 3 eggs in the shopping cart"
console.log(formatMessage("egg", 1)); // "You have 1 egg in the shopping cart"
```

> Dentro de la interpolación podemos añadir cualquier expresión, ya sea operadores ternarios, de asignación, de comparación, invocaciones a funciones, etc.
