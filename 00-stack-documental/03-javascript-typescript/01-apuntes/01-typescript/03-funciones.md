# Funciones

## Introducción

Tipar una función en TypeScript no es más que especificar los tipos
de los argumentos que recibe y el tipo de dato que devuelve.
Es importante tener en cuenta que el número de argumentos que
especifiquemos son obligatorios.

```ts
function shout(text: string, upperCase: boolean): string {
  return (upperCase ? text.toUpperCase() : text) + "!!!";
}

const t1 = shout("hi"); // [ts] Expected 2 arguments, but got 1
const t2 = shout("hi", true);
console.log(t2); // "HI!!!"
```

Su homólogo en arrow function:

```ts
const shout = (text: string, upperCase: boolean): string => (upperCase ? text.toUpperCase() : text) + "!!!";

const t3 = shout("hi"); // [ts] Expected 2 arguments, but got 1
const t4 = shout("hi", false);
console.log(t4); // "hi!!"
```

## Argumentos opcionales

Utilizando el operador `?` sobre un argumento significa que dicho
argumento es opcional a la hora de invocar a la función

<!-- prettier-ignore -->
```ts
const shout = (text: string, upperCase?: boolean): string =>
  (upperCase ? text.toUpperCase() : text) + "!!!";

// Si no pasamos explícitamente un argumento opcional su valor es,
// al igual que en JavaScript, undefined.
console.log(shout("hi")); // "hi!!!" ---> upperCase = undefined.
```

## Argumentos por defecto

También es posible declarar el tipo de valores por defecto, aunque
por lo general es más legible el no declarar el tipo y dejar que
TypeScript lo infiera.
[!] No se puede mezclar el operador opcional con valores por defecto
aunque al inspeccionar el tipo ya es opcional

<!-- prettier-ignore -->
```ts
const shout = (text: string, upperCase: boolean = true): string =>
  (upperCase ? text.toUpperCase() : text) + "!!!";

console.log(shout("hi")); // "HI!!!"
```

## Alias

En estos ejemplos vistos, el tipado de la funcion va integrado en
la propia declaración/definición de la función.
Sin embargo, podemos extraer el typo de una función aparte, y
reusarlo cuando queramos. Para ello usamos el operador "type".
Esto se conoce como ALIAS y lo veremos un poco más adelante:

<!-- prettier-ignore -->
```ts
type ShoutFunction = (text: string, upperCase: boolean) => string;
const shout: ShoutFunction = (text, upperCase) =>
  (upperCase ? text.toUpperCase() : text) + "!!!";

console.log(shout("TS rocks", true));
```

## Funciones como argumentos

También es posible tipar argumentos que son funciones:

<!-- prettier-ignore -->
```ts
const shout = (text: string, getNumExclamation: () => number): string =>
  text.toUpperCase() + "!".repeat(getNumExclamation());

const getRandom = () => Math.ceil(Math.random() * 10); // Este es mi callback.

console.log(shout("WoW", getRandom));
console.log(shout("WoW", getRandom));
console.log(shout("WoW", getRandom));
console.log(shout("WoW", getRandom));
console.log(shout("WoW", getRandom));
console.log(shout("WoW", getRandom));
```
