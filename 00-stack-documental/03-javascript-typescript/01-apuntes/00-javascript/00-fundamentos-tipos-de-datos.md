# Fundamentos

## Tipos de datos

Distinguimos 2 grandes grupos en Javascript:

- Tipos primitivos (representan un único dato simple).
- Tipos estructurales (representan estructuras de datos) u objetos.

### Tipos primitivos

Un tipo primitivo es aquel que no es un objeto y por tanto no tiene métodos. Representan datos simples, sencillos. Existen 7 tipos primitivos.

Todos los primitivos son inmutables. Una vez creado un valor primitivo no puede ser alterado ni modificado (no confundir con reasignar una variable con otro valor).

#### string

Representa una cadena de caracteres UTF-16. Se pueden representar tanto con comillas simples como dobles.

<!-- prettier-ignore -->
```js
// Comillas dobles
"hello world"
""
// Comillas simples
"hello world"
''
```

#### number

Representan tipos de datos numéricos enteros o decimales. Se implementan en punto flotante de preisión 64 bits. Cualquier operación sobre números mayores a `Number.MAX_SAFE_INTEGER` o menores a `Number.MIN_SAFE_INTEGER` pueden devolver resultados imprecisos.

<!-- prettier-ignore -->
```js
// Números finitos
101       // entero positivo
-200      // entero negativo
1220.31   // flotante positivo
-1220.31  // flotante negativo

// Notación binaria
0b101     // notación binaria (5)
0o4       // notación octal (4)
0xFF      // notación hexadecimal (255)
1e6       // notación exponencial (1 x 10^6)

// Con separadores numéricos. Se pueden añadir separadores numéricos en cualquier punto.
// Los siguientes ejemplos representan el número mil.
1_000
1_0_0_0
100_0
10_00

// Números no finitos
Infinity  // Infinito
-Infinity // Menos infinito
NaN       // "Not a number" (de hecho, es de tipo número)
```

#### bigint

Representan números que pueden ir por encima de 2^53 - 1.

<!-- prettier-ignore -->
```js
// Números finitos
101n       // entero positivo
-200n      // entero negativo

// Notación binaria
0b101n     // notación binaria (5)
0o4n       // notación octal (4)
0xFFn      // notación hexadecimal (255)
```

#### boolean

Representan dos posibles valores en álgebra de Boole: verdadero o falso.

<!-- prettier-ignore -->
```js
true
false
```

#### null

Representa la ausencia de valor de forma intencional. Es un primitivo especial de tipo "object".

<!-- prettier-ignore -->
```js
null
```

#### undefined

Representa la ausencia de valor que representa algo que no existe, a diferencia de `null` que se puede interpretar como que existe pero con valor vacío.

<!-- prettier-ignore -->
```js
undefined
```

#### symbol

Representa un valor único (o un identificador) etiquetado de forma opcional con un string.

<!-- prettier-ignore -->
```js
// Creación de un símbolo
Symbol()

// Creación de un símbolo con etiqueta
Symbol('hello')

// Acceso o creación de un símbolo en el global registry
Symbol.for('hello')
```

### Checkeo de tipos

Para comprobar el tipo de datos se utiliza el operador `typeof`.

<!-- prettier-ignore -->
```js
typeof "hello"    // "string"
typeof 0          // "number"
typeof 30n        // "bigint"
typeof null       // "object"
typeof undefined  // "undefined"
typeof true       // "boolean"
typeof Symbol()   // "symbol"
```

## Tipos estructurales (objetos)

Representan aquellos valores que no son primitivos.

### Objetos

Son estructuras de datos clave-valor. Se ven con más detalle [en secciones posteriores ](./05-estructuras-de-datos.md).

<!-- prettier-ignore -->
```js
// Objeto literal vacío
{}

// Tipo de dato
typeof {}   // "object"
```

### Arrays

Son estructuras de datos que almacenan otros tipos de datos de forma ordenada. Se ven con más detalle [en secciones posteriores ](./05-estructuras-de-datos.md).

<!-- prettier-ignore -->
```js
// Array literal vacío
[]

// Tipo de dato
typeof []   // "object" (los arrays son objetos!)
```

### Funciones

Son estructuras de datos que pueden ser invocadas con un número de argumentos dinámicos y devolver valores.

<!-- prettier-ignore -->
```js
// Declaración
function main(arg) {
  console.log(arg);
  return arg;
}

// Invocación
main("hello");

// Tipo de dato
typeof main   // "function" (las funciones son un tipo especial de objeto!)
```
