# Anotación de tipos

## Introducción

Para anotar tipos con Typescript usamos los puntos (:), como si del valor
de una propiedad se tratase. Asi pues, para tipar una variable, añadimos
su anotación de tipo a continuación del nombre de la variable:

```ts
const city: string = "Málaga";
const num: number = 3;
```

TS hace chequeo de tipos al transpilar y nos informa de errores y advertencias.
Además, si tenemos instalado un linter para TS como tslint, este chequeo
se realiza en vivo y nos avisa de posibles errores en nuestro editor de
código.

```ts
const text: string = 3; // [ts] Type '3' is not assignable to type 'string'
const pi: number = "3.1415"; // [ts] type '"3.1415"' is not assignable to type 'number'
text = "reassigned"; // [ts] Cannot assign 'text' because it is a constant or a read-only property
```

## Tipos básicos

### boolean

```ts
const fake: boolean = true;
```

### number

```ts
const integer: number = 13; // 13
const float: number = 13.13; // 13.13
const hex: number = 0xd; // 13
const binary: number = 0b1101; // 13
const octal: number = 0o15; // 13
```

### string

```ts
const city: string = "Málaga";
let phrase: string = "¿Qué tiempo hace?";
phrase = "No muy bueno";
phrase = `Dan lluvia en ${city}`;
```

### array

Se puede anotar su tipo con 2 variantes, los corchetes [] para expresar
array o mediante el tipo genérico Array especializado al tipo concreto de
array.

```ts
const coins: number[] = [1, 2, 0.5];
let words: Array<string> = [];
words.push("hello");
words.push("world");
words.push({}); // [ts] Argument of type '{}' is not assignable to parameter of type 'string'
```

### tuple

Una tupla no es más que es un array de longitud fija.

```ts
// Array con tipos heterogéneos
const quantity: [number, string] = [5, "pieces"];
console.log(quantity[1].toUpperCase());
console.log(quantity[0].toUpperCase()); // [ts] Property 'toUpperCase' does not exists in type 'number'
```

### enum

Un enumerado es una estructura que define una serie de constantes que representan un mismo tipo.
Pueden contener valores de tipo `number` o `string`.

Ejemplo de enumerado usando enteros por defecto:

```ts
enum DiaSemana { // Por convención, los tipos no primitivos siempre en PascalCase
  Lunes, // = 0
  Martes, // = 1
  Miercoles, // = 2
  Jueves, // = 3
  Viernes, // = 4
  Sábado, // = 5
  Domingo, // = 6
}
const dia: DiaSemana = DiaSemana.Miercoles;
console.log(dia); // 2
```

Si no indico nada, por defecto el enum se enumera con un índice entero
base 0. Aunque puedo indicar, si deseo, el primer índice, o incluso todos:

```ts
enum DiaSemanaUSA {
  Lunes = 1,
  Martes,
  Miercoles,
  Jueves,
  Viernes,
  Sábado,
  Domingo = 0,
}
const diaUSA: DiaSemanaUSA = DiaSemanaUSA.Domingo;
console.log(diaUSA); // 0
```

Los enumerados se pueden usar como diccionaros string <-> number:

```ts
enum Release {
  Beta = 0.1,
  ProductLaunch = 1.0,
  ImprovedSupport = 1.4,
  NewYearPackage = 2.0,
}
const rel: Release = Release.ImprovedSupport;
console.log(rel); // 1.4
const relName: string = Release[1.4];
const relName1: string = Release[Release.ImprovedSupport];
console.log(relName); // ImprovedSupport
```

Ejemplo de un enumerado con valores en string:

```ts
enum MediaTypes {
  JSON = "application/json",
  XML = "application/xml",
  PLAIN = "text/plain",
}

const jsonMedia: MediaTypes = MediaTypes.JSON;
console.log(jsonMedia); // "application/json"
// [!] El identificador de un enumerado cuyo valor es un string no se puede
// obtener mediante su valor
```

> IMPORTANTE: Un enumerado de tipo string no tiene _reverse-mapping_, es decir, no podemos acceder al nombre que referencia al valor.
> Esto es debido a que una constante podría tener como nombre el valor de otra constante y podríamos machacar accidentalmente el valor.

### void

El tipo `void` representa el valor de retorno de las funciones que no devuelven ningún valor. Es equivalente a `undefined`.

```ts
function noReturn() {
  console.log("I am not going to return anything");
}

const result: void = noReturn();
const notAssigned: void = undefined;
```

### null y undefined

En TypeScript existen tanto "null" como "undefined" como tipos propios e
independientes:

```ts
const u: undefined = undefined; // Sólo puedo asignarle undefined o null.
const n: null = null; // Sólo puedo asignarle null o undefined.
```

Sin embargo, dependiendo de si tenemos habilitado o no el modo estricto en las opciones del compilador
podremos asignar `null` o `undefined` a otros tipos de datos:

```ts
const nullNumb: number = null;
const undefString: string = undefined;
let whatever: any = undefined;
whatever = null;
```

### object

Representa a los tipos custom, aquellos que no son primitivos. Es decir,
todo lo que no sea "number", "string", "boolean", "null" o "undefined".

```ts
const obj: object = {};
```

### never

Representa a valores que nunca van a ocurrir. Es un tipo abstracto útil para definiciones de tipos más completas.

```ts
// Esta función no llegará a devolver `void` porque lanzará un error
const throwError = () => {
  throw new Error("Oops!");
};

const result1: never = throwError();

// Esta función no llegará a devolver `void` porque nunca llegará a terminar
const neverEndingFunction = () => {
  while (true) {} // Infinite loop.
};

const result2: never = neverEndingFunction();
```

### any

Es un tipo especial que representa cualquier valor.
Es útil cuando no queremos que TypeScript lance errores, ya que que deshabilita el chequeo de tipos.

```ts
let myData: any;
myData = "Maybe is a string";
myData = false;
myData = {};
myData = () => console.log("I am a function now");
myData.push(3); // Ok, myData podría ser un array y por tanto .push() podría existir.
myData.toExponential(2); // Ok, myData podría ser un numero y por tanto .toExponential() podría ser válido.

let falsyValues: any[] = [0, undefined, null, NaN, false];
falsyValues.push("");
```

El tipo `any` es potente y a la vez peligroso.
Puede ser útil si sabemos lo que hacemos pero también abrimos la puerta a posibles erroes.
Es lo más parecido a volver a vanilla JavaScript.

### unknown

Es un tipo especial que representa cualquier valor. A diferencia de `any` es un tipo
que fuerza al desarrollador a desambiguar el tipo antes de realizar cualquier operación.

```ts
const data: unknown = "Maybe is a string";
data.toUpperCase(); // [ts] Object is of type 'unknown'

if (Array.isArray(data)) {
  data.push("hello");
}
```

## Type assertion o aseveración de tipos

Hay veces en las que tenemos la certeza de conocer el tipo de un determinado
valor, que Typescript no conoce. En dichos casos podemos aseverar el tipo
por nosotros mismos, y decirle a TypeScript, "oye, confia en mi, sé lo que hago".
Volvamos al ejemplo del any:

```ts
let data: any;
data = "I am a string";
```

Es probable que en algún punto en el código sepamos con certeza el tipo que contiene la variable
pero TypeScript no. Podemos cambiar el tipo de dato de la variable a otro mediante un casting:

```ts
// Intellisense no funciona asi, porque data es de tipo any.
console.log(data.substr(0));

// Casting usando `as`. Intellisense si funciona ahora, reconoce el tipo.
console.log((data as string).substr(0));

// Casting usando ángulos. Intellisense si funciona ahora, reconoce el tipo.
console.log((<string>data).substr(1));
```
