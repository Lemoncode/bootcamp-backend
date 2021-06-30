# Destructuring

Destructuring es una técnica rápida para asignar propiedades de objetos a variables, o items de un array a variables.

## Sobre objetos

Supongamos el siguiente objeto:

```js
const student = {
  name: "Evan",
  surname: "Smith",
  country: {
    id: 21,
    name: "Spain",
    iso3: "SPA",
  },
};
```

Podemos extraer rápidamente los valores de sus propiedades a variables mediante destructuring, sólo aquellos que nos interesen, por ejemplo:

```js
const { name, surname } = student; // Destructuring

console.log(name); // "Evan"
console.log(surname); // "Smith"
```

En la sentencia de destructuring también puedo renombrar las variables que genero:

```js
const { name: studentName, surname: studentSurname } = student;
console.log(studentName); // "Evan"
console.log(studentSurname); // "Smith"
```

Se puede hacer destructuring en profundidad (sobre objetos andiados):

<!-- prettier-ignore -->
```js
const { name, country: { name: countryName, iso3 } } = student;
console.log(name); // "Evan"
console.log(countryName); // "Spain"
console.log(iso3); // "SPA"
```

El destructuring resulta realmente útil sobre argumentos de una función:

```js
const getName = ({ name }) => name;
console.log(getName(student)); // "Evan"
```

## Sobre arrays

Supongamos el siguiente array:

```js
const students = ["Alan", "Evan", "Ana"];
```

Podemos extraer rápidamente elementso del array a variables mediante destructuring, sólo aquellos que nos interesen, por ejemplo:

```js
const [first, second, third, fourth] = students;
console.log(first); // "Alan"
console.log(second); // "Evan"
console.log(third); // "Ana"
console.log(fourth); // undefined
```

Podemos hacer target a elementos intermedios, omitiendo los anteriores, mediante la coma:

```js
const [, , third] = students;
console.log(third); // "Ana"
```

Al igual que con los objetos, se emplea mucho en argumentos de función:

```js
const getSecond = ([, second]) => second;
console.log(getSecond(students)); // "Evan"
```

Y también se puede aplicar destructuring en profundidad:

```js
const matrix = [
  [0, 0, 0],
  [0, 10, 0],
  [0, 0, 0],
];

const [, [, center]] = matrix;
console.log(center); // 10;
```
