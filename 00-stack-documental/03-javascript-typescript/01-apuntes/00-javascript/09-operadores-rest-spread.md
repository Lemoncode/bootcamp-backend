# Operadores Rest y Spread

En javascript contamos con dos operadores especiales, frecuentemente utilizados, que se notan con el mismo símbolo, los 3 puntos (...), pero cuyo comportamiento varía según donde se utilicen, haciendo la función de _rest_ o _spread_ según el caso.

## Con arrays

Podemos extender (_spread_) los valores de un array en un nuevo array:

```js
const original = ["one", "two", "three", { name: "Santi" }];
const copy = [...original]; // Extendemos cada elemento del array origen en el array destino

console.log(original); // ["one", "two", "three", {name: "Santi"}]
console.log(copy); // ["one", "two", "three", {name: "Santi"}]
```

¿Es el mismo array? NO, es un nuevo array copiado de forma poco profunda: _shallow copy_. No se clonan los objetos sino que se copian sus referencias.

```js
console.log(original === copy); // false
console.log(original[0] === copy[0]); // true
console.log(original[3] === copy[3]); // true
```

Hay que tener cuidado con las _shallow copies_ ya que si muto alguno de los objetos en la copia, afecará al original:

```js
copy[3].age = 30;
console.log(copy[3]);
console.log(original[3]);
```

Se suele emplear _spread_ para extender elementos de un array como argumentos de una función:

```js
const main = (first, second, third) => console.log(first, second, third);
main(...original);
```

El operador _rest_ hace justo lo contrario: agrupar elementos en un array. Por ejemplo, podemos generar un array con todos los argumentos pasados a una funcion. ¿Os acordáis de que las lambdas no tenían _keyword_ `arguments`? Se puede emular con _rest_:

```js
const sum = (...nums) => nums.reduce((acc, num) => acc + num, 0);
console.log(sum()); // 0
console.log(sum(1, 2, 3)); // 6
```

Un uso muy interesante de _rest_ es la capacidad de aislar argumentos iniciales y agrupar los restantes, es decir, excluir:

```js
const sumAllButFirst = (_first, ...nums) => nums.reduce((acc, num) => acc + num, 0);
console.log(sumAllButFirst()); // 0
console.log(sumAllButFirst(1, 2, 3)); // 5
```

## Con objetos

Podemos extender (_spread_) las propiedades de un objeto en un nuevo objeto:

```js
const original = {
  name: "Evan",
  surname: "Smith",
  country: {
    id: 21,
    name: "Spain",
    iso3: "SPA",
  },
};
const copy = { ...original }; // Extendemos cada propiedad del objeto origen en el objeto destino

console.log(original);
console.log(copy);
```

Al igual que antes no se clonan las propiedades sino que se copian de forma poco profunda (_shallow copy_), por lo que hay que tener cuidado con las mutaciones:

```js
console.log(original === copy); // false
console.log(original.name === copy.name); // true
console.log(original.country === copy.country); // true

delete copy.country.iso3;
console.log(original.country); // {id: 23,name: "Spain"}
console.log(copy.country); // {id: 23,name: "Spain"}
```

Una combinación potente consiste en aplicar _destructuring_ y _rest_ para eliminar propiedades de objetos de forma inmutable, cuando se pasan como argumento de una función. Es decir, en lenguaje llano, para "quedarnos con lo que nos interesa":

```js
const excludeCountry = ({ country, ...others }) => others;
const nameAndSurname = excludeCountry(original);
console.log(nameAndSurname); // {name: "Evan", surname: "Smith"}
```
