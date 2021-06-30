# Estructuras de datos

## Objetos

Los objetos representan datos estructurados siguiendo el formato clave-valor. A cada clave o alias lo llamamos propiedad.

Inicialización de objetos de forma literal, "object literals":

```js
const person = { name: "John" }; // {} => inicializador de objetos
```

Las propiedades de un objeto también pueden inicializarse a partir de variables existentes:

```js
const name = "John";
const person = { name: name };
```

Si los nombres de la propiedad y la variable coinciden, se puede expresar de forma corta:

```js
const person = { name };
```

Accediendo a propiedades:

```js
console.log(person.name); // "John"
console.log(person["name"]); // "John". Útil cuando el nombre de la propiedad nos viene dado por una variable.
console.log(person.lastname); // undefined
```

Añadiendo nuevas propiedades:

```js
person.lastname = "Smith";
console.log(person.lastname); // "Smith"
person[21] = "twenty one";
console.log(person["21"]); // "twenty one"

// Las propiedades pueden ser a su vez otros objetos
person.country = { id: 5, name: "Spain" };
console.log(person.country); // { id: 5, name: "Spain" }

// Y también pueden ser funciones
person.greet = function () {
  console.log("Hello!");
};
console.log(person.greet); // function() { console.log("Hello!"); }
person.greet(); // logs "Hello!"
person["greet"](); // logs "Hello!"
```

Iterando por las propiedades:

> Orden de iteración: first assigned => first shown, excepto para claves numéricas que se mostrarán en primer lugar

```js
for (const prop in person) {
  console.log(prop, person[prop]);
}
// "21" "twenty one"
// "name" "John"
// "lastname" "Smith"
// "country" {id: 5, name: "Spain"}
// "greet" function() { console.log("Hello!"); }
```

Borrando propiedades:

```js
delete person.lastname;
console.log(person.lastname); // undefined
delete person.country.id;
console.log(person.country); // { name: "Spain" }
```

Comparando objetos:

> IMPORTANTE: Se utiliza igualdad referencial

```js
const boy = { age: 15 };
console.log(boy === { age: 15 }); // [!!!] false. Se comparan REFERENCIAS, no contenido.
console.log(boy === boy); // true
console.log(boy.toString()); // [object Object]
```

## Arrays

Los arrays representan datos estructurados siguiendo un orden. Cada dato se identifica con un índice que indica su posición dentro de la estructura.

Inicialización de arrays de forma literal:

```js
const collection = ["hey", "ho", "let's go"]; // [] => Inicializador de arrays
```

Accediendo a sus elementos:

```js
console.log(collection[0]); // "hey"
console.log(collection[3]); // undefined
console.log(collection.length); // 3
```

Un array puede contener cualquier tipo de elemento:

```js
const mixedCollection = [1, 2, 3, "Go!", { object: true }];
```

Los arrays, en realidad, se implementan mediante objetos en javascript, usando claves numéricas que indican el índice de los valores que almacena. Equivalencia con un objeto:

```js
const collection = {
  0: "hey",
  1: "ho",
  2: "let's go",
  length: 3,
};
```

Añadiendo elementos al array:

```js
collection.push("yay!");
console.log(collection); // ["hey", "ho", "let's go", "yay"]
collection[4] = "nice";
console.log(collection); // ["hey", "ho", "let's go", "yay", "nice"]
console.log(collection.length); // 5

// Sparse array: Solo almacena en memoria los valores que hayan sido asignados
collection[100] = "oops!";
console.log(collection); // ["hey", "ho", "let's go", "yay", "nice", empty x95, "oops!"]
console.log(collection.length); // 101
```

Iterando por los elementos de un array:

```js
// forEach()
collection.forEach(function (value) {
  console.log(value); // "hey", "ho", "let's go", "yay", "nice", "oops!"
});

// for(...)
for (let i = 0; i < collection.length; i++) {
  console.log(collection[i]); // "hey", "ho", "let's go", "yay", (x95) undefined, "oops!"
}

// for..of
for (const item of collection) {
  console.log(item); // "hey", "ho", "let's go", "yay", (x95) undefined, "oops!"
}
```

Comparando arrays

> IMPORTANTE: Los arrays son objetos y por tanto implementan la misma comparación que éstos: igualdad referencial

```js
const collection = [3];
console.log(collection === [3]); // false. Different object.
console.log(collection === collection); // true
// Curiosidad
console.log([] == ""); // true (type coertion). [].toString() => "" == ''
```
