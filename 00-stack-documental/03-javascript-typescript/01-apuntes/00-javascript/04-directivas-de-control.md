# Directivas de control

## Control de flujo

### if-else

Permiten ejecutar un bloque de código si se cumple una condición. Si no se cumple se puede ejecutar otro bloque de código.

Directiva `if` con una sóla rama:

```js
const count = 0;
if (count === 0) {
  console.log("zero");
}
```

Con dos ramas:

```js
if (count === 0) {
  console.log("zero");
} else {
  console.log("non-zero");
}
```

Con múltiples ramas:

```js
if (count === 0) {
  console.log("zero");
} else if (count === 1) {
  console.log("one");
} else {
  console.log("more than one");
}
```

Si el bloque de código consta de una sola expresión podemos opcionalmente eliminar las llaves:

```js
if (count === 0) console.log("zero");
else if (count === 1) console.log("one");
else console.log("more than one");
```

### switch

Similar a la directiva `if-else` donde en vez de comprobar una condición comprobamos valores:

```js
const pet = "dog";
switch (pet) {
  case "cat":
    console.log("medium pet");
    break;
  case "dog":
    console.log("large pet");
    break;
  case "bird":
    console.log("small pet");
    break;
  default:
    console.log("unknown size");
}
```

La directiva `break` evita que diferentes bloques de código de los `case` se ejecuten. Podemos reutilizar los casos de forma conveniente si no lo ponemos:

```js
const pet = "dog";
switch (pet) {
  case "cat":
  case "dog":
    console.log("mammal");
    break;
  case "bird":
  default:
    console.log("non-mammal");
}
```

### Operador ternario

Expresión similar a `if-else` que devuelve un valor directamente. Es útil para ser usados en retorno de funciones o asignación de variables:

<!-- prettier-ignore -->
```js
const age = 20;
const status = (age >= 18) ? "adult" : "minor";
```

Los paréntesis son opcionales:

```js
const status = age >= 18 ? "adult" : "minor";
```

Podemos anidar ternarios. Hay que tener cuidado porque puede afectar la legibilidad de código.

<!-- prettier-ignore -->
```js
const status = age >= 18 ? "adult" : (age >= 14 ? "teen" : "kid");
```

## Iteradores

### for

Se divide en tres bloques: inicializador, condición y expresión final:

```js
const limit = 10;
for (let i = 0; i < limit; i++) {
  console.log(i);
}
```

Múltiples asignaciones en el inicializador

```js
for (let i = 0, limit = 10; i < limit; i++) {
  console.log(i);
}
```

> IMPORTANTE: Cuidado con usar `const` en el inicializador.

### while

Iteran mientras ocurra la condición. Siempre se evalúa la condición antes de ejecutar el bloque.

```js
const limit = 10;
let i = 0;
while (i < limit) {
  console.log(i);
  i++;
}
```

### do while

Similar a `while` pero evalúa la condición tras ejecutar el bloque.

```js
const limit = 10;
let i = 0;
do {
  console.log(i);
  i++;
} while (i < limit);
```

### for..in

Explicado con los objetos en la sección de [estructuras de datos](./05-estructuras-de-datos.md).

### forEach

Explicado con los arrays en la sección de [estructuras de datos](./05-estructuras-de-datos.md).

## Operador coma

Podemos crear múltiples expresiones encerradas entre paréntesis y separadas por coma para crear una única expresión que devolverá la última expresión de los paréntesis. Aunque su uso es poco habitual e ilegible, es útil para hacer código compacto.

<!-- prettier-ignore -->
```js
const a = (2 + 4, 9);
console.log(a); // 9
let b = 3;
const c = (b += 5, 10);
console.log(a); // 9
console.log(b); // 8;
console.log(c); // 10;
```
