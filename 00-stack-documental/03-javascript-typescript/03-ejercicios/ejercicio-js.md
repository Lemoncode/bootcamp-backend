# Ejercicios TypeScript

## Ejercicio 1

Un texto en formato CSV tiene el nombre de los campos en la primera fila y los datos en el resto.
Crea un parseador de CSV a JSON que sólo coja los tres primeros campos.
Utiliza destructuring, rest y spread operator donde creas conveniente.

<!-- prettier-ignore -->
```js
const data = `id,name,surname,gender,email,picture
15519533,Raul,Flores,male,raul.flores@example.com,https://randomuser.me/api/portraits/men/42.jpg
82739790,Alvaro,Alvarez,male,alvaro.alvarez@example.com,https://randomuser.me/api/portraits/men/48.jpg
37206344,Adrian,Pastor,male,adrian.pastor@example.com,https://randomuser.me/api/portraits/men/86.jpg
58054375,Fatima,Guerrero,female,fatima.guerrero@example.com,https://randomuser.me/api/portraits/women/74.jpg
35133706,Raul,Ruiz,male,raul.ruiz@example.com,https://randomuser.me/api/portraits/men/78.jpg
79300902,Nerea,Santos,female,nerea.santos@example.com,https://randomuser.me/api/portraits/women/61.jpg
89802965,Andres,Sanchez,male,andres.sanchez@example.com,https://randomuser.me/api/portraits/men/34.jpg
62431141,Lorenzo,Gomez,male,lorenzo.gomez@example.com,https://randomuser.me/api/portraits/men/81.jpg
05298880,Marco,Campos,male,marco.campos@example.com,https://randomuser.me/api/portraits/men/67.jpg
61539018,Marco,Calvo,male,marco.calvo@example.com,https://randomuser.me/api/portraits/men/86.jpg`;

const csvToJSON = (csv) => {

};

const result = csvToJSON(data);
console.log(result);

/*

Formato del resultado:

[
  {
    "id": "15519533",
    "name": "Raul",
    "surname": "Flores",
    "gender": "male",
    "email": "raul.flores@example.com",
    "picture": "https://randomuser.me/api/portraits/men/42.jpg"
  },
  {
    ...
  }
]
*/
```

Opcional: Añade un segundo argumento a la función que sea el número de atributos a añadir. Si no se le pasa el número
de atributos debe de añadir todos:

```js
const csvToJSON = (csv, nAttrs) => {};

console.log(csvToJSON(data)); // Cada usuario tiene todos los atributos como la implementación original
console.log(csvToJSON(data, 2)); // cada usuario tendrá sólo `id` y `name`
console.log(csvToJSON(data, 3)); // cada usuario tendrá sólo `id`, `name` y `surname`
console.log(csvToJSON(data, 4)); // cada usuario tendrá sólo `id`, `name`, `surname` y `gender`
```

## Ejercicio 2

Usando spread operator y `findIndex`

Implementar una funcion `replaceAt` que tome como primer argumento un array, como segundo argumento un índice
y como tercer argumento un valor y reemplace el elemento dentro del array en el índice indicado.
**El array de entrada no debe de ser mutado**, eso es, que debes de crear un nuevo array.
Utiliza _spread operator_, `slice` y `findIndex` para conseguirlo.

<!-- prettier-ignore -->
```js
const elements = ["lorem", "ipsum", "dolor", "sit", "amet"];
const index = 2;
const newValue = "furor";

const replaceAt = (arr, index, newElement) => {

};

const result = replaceAt(elements, index, newValue);
console.log(result === elements); // false
console.log(result); // ['lorem', 'ipsum', 'furor', 'sit', 'amet'];
```

## Ejercicio 3

Arrow functions con map + filter? Añadir template literal

## Ejercicio 4

Crear funcion para normalizar un array a objeto

## Ejercicio 5

Implementa una función para eliminar valores falsys de un objeto. Implementa otra para los arrays
