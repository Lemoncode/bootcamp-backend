# Operadores básicos

## Operadores aritméticos

```js
// Suma
console.log(52 + 21); // 73

// Concatenación
console.log("hello " + "world"); // "hello world"

// Resta
console.log(10 - 5); // 5

// Multiplicación
console.log(10 * 10); // 100;

// División
console.log(9 / 3); // 3

// División que devuelve decimales
console.log(15 / 2); // 7.5;

// Módulo o resto
console.log(15 % 3); // 0

// Exponenciación
console.log(2 ** 3); // 8
```

## Operadores de asignación

```js
let num = 3;

// Post-incremento (se incrementa en uno después de su uso)
console.log(num++); // 3

// Post-decremento (se decrementa en uno espués de su uso)
console.log(num--); // 4

// Pre-incremento (se incrementa en uno antes de su uso)
console.log(++num); // 4

// Pre-decremento (se decrementa en uno antes de su uso)
console.log(--num); // 3

// Asignación de incremento
num += 5; // Equivalente a num = num + 5
console.log(num); // 8

// Asignación de decremento
num -= 5; // Equivalente to num = num - 5
console.log(num); // 3

// Asignación de producto
num *= 10; // Equivalente a num = num * 10
console.log(num); // 30

// Asignación de división
num /= 6; // Equivalente a num = num / 6
console.log(num); // 5

// Asignación de módulo o resto
num %= 3; // Equivalente a num = num % 3
console.log(num); // 2

// Asignación de exponenciación
num **= 10; // Equivalente a Math.pow(2, 10) o diez veces num * 10
console.log(num); // 1024
```

## Operadores de comparación

```js
// Mayor que
console.log(3 > 0); // true
console.log(3 > 3); // false

// Menor que
console.log(3 < 0); // false
console.log(3 < 3); // false

// Mayor o igual que
console.log(3 >= 3); // true

// Menor o igual que
console.log(3 <= 3); // true

// Igualdad débil
console.log(5 == 5); // true

// Igualdad estricta
console.log(5 === 5); // true
```

## Igualdad y coerción de tipos

JavaScript es un lenguaje de tipado fuerte o estático, por lo que se pueden comparar miembros de distinta naturaleza. En tal caso, la estrategia que sigue JavaScript es convertir implicitamente uno de los miembros o los dos a un tipo común para poder realizar la comparativa. A esto se le llama **type coercion** o **conversión implícita/automática** (ver [referencia](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Equality_comparisons_and_sameness#Loose_equality_using)).

<!-- prettier-ignore -->
```js
console.log(5 == "5");    // true // [!] Loose equality. Igualdad débil. (Por type coertion, "5" string se convierte a 5 numero)
console.log(5 === "5");   // false // [!] Strict equality. Igualdad fuerte.
console.log(5 != 5);      // false
console.log(5 != "5");    // false. (Por type coercion, "5" string se convierte a 5 numero)
console.log(5 !== 5);     // false
console.log(5 !== "5");   // true
console.log(0 == false);  // true. (Por type coercion, false se castea a 0)
console.log(0 === false); // false. (number != boolean)
```

## Operadores lógicos

<!-- prettier-ignore -->
```js
// && AND
console.log(true && true);    // true
console.log(true && false);   // false
console.log(false && true);   // false
console.log(false && false);  // false

// || OR
console.log(true || true);    // true
console.log(true || false);   // true
console.log(false || true);   // true
console.log(false || false);  // false
```

**IMPORTANTE**. De nuevo, JavaScript puede tener operandos de distinta naturaleza. Los operadores `&&` y `||`, cuando se usan con operandos no booleanos pueden devolver un resultado no booleano, cualquiera: array, objeto.

Para determinar qué valor será devuelto dependerá del operador que utilicemos:

- El operador OR `||` devolverá el valor de la izquierda si es un valor _truthy_, de lo contrario, devolverá el valor de la derecha.
- El operador AND `&&` devolverá el valor de la izquierda si es un valor _falsy_, de lo contrario, devolverá el valor de la derecha.

Existen 6 valores _falsys_ en JavaScript.

<!-- prettier-ignore -->
```js
null
undefined
NaN
0
""
false
```

Veamos ejemplos de aplicación.

```js
var a = 3 || 20; // 3. El 3 es el primer valor "truthy" que se encuentra el OR.
var a = 0 || 20; // 20. El 20 es el primer valor "truthy" que se encuentra el OR.
var a = Boolean(0 || 20); // true
var a = 3 && 20; // 20
var a = 0 && 20; // 0
var a = Boolean(0 && 20); // false
var a = 2 > 0 && "hello"; // "hello"
var a = 2 < 0 && "hello"; // false
```
