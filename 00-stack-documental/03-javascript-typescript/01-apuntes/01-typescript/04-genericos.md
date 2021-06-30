# Tipos genéricos

## Introducción

Hay muchas veces que no sabemos con qué tipo de datos nos vamos a
encontrar, especialmente utilizando colecciones. Sin tipos genéricos
tendríamos que recurrir al tipo any y, como ya hemos visto, perdemos
completamente el tipado de nuestras variables. Es por ello por lo que
existe el tipo genérico (Semejante al de Java o C#).

## Genéricos en funciones

```ts
// En funciones clásicas
function first<T>(list: T[]): T {
  return list[0];
}

// En arrow functions
const firstArrow = <T>(list: T[]): T => list[0];
```

Podemos especificar el tipo genérico a la hora de invocar la función:

```ts
const res1 = first<string>(["hello", "world"]); // res1 es de tipo string
const res2 = firstArrow<null>([null, null]); // res2 es de tipo string
const res3: string = firstArrow<number>([1, 2, 3]); // [ts] Type 'number' is not assignable to type 'string'
```

Si no especificamos el tipo genérico TypeScript hará todo lo posible para inferir
el tipo basándonos en su uso:

```ts
const res3 = first([1, 2, 3, 4, 5]); // res3 es un number
const res4 = firstArrow([false, "0", true, "1"]); // res4 es un string o un boolean
```

Veamos cómo tipar una variable cuyo tipo es una función que utiliza genéricos:

```ts
const myTypedFunc: <T>(list: T[]) => T = first;

// Incluso podríamos usar una letra distinta a la empleada en la
// declaración incial:
const myTypedFuncAlt: <U>(list: U[]) => U = first;
```

## Genéricos en interfaces

```ts
interface State<T> {
  value: T;
}

const stringState: State<string> = { value: "stored" };
```

A diferencia de las funciones, los tipos genéricos de las
interfaces se tienen que definir en su uso por obligación.

```ts
const namesState: State = { value: [] }; // [ts] Generic type 'State<T>' requires 1 type argument(s)

function isStateEmpty<T>(state: State<T>): boolean {
  return !!state.value;
}

const emptyStringState = isStateEmpty({ value: "" }); // emptyStringState es de tipo boolean
```

## Múltiples genéricos

Aunque los ejemplos anteriores los hemos expuesto
con un solo genérico por sencillez, se pueden utilizar tantos
como sean necesarios, en forma de lista separada por comas entre
los ángulos. Por ejemplo:

```ts
const isSameValue = <T, U>(arg1: T, arg2: U): boolean => typeof arg1 === typeof arg2;

console.log(isSameValue<string, number>("1", 1));
console.log(isSameValue<string, string>("halloween", "halloween"));
```
