# Argumentos por defecto

En javascript es posible asignar valores por defecto a argumentos de una función. De este modo, dispondremos de un valor para un argumento que no ha sido informado en la llamada:

```js
const greet = (name = "Unknown") => console.log("Hello, " + name);

greet("Jake"); // Argumento name informado -> "Hello, Jake"
greet(); // Argumento name NO informado -> "Hello, Unknown"

// Los valores por defecto SOLO son aplicados si el argumento es específicamente undefined (no informado)
greet(undefined); // "Hello, Unknown"
greet(null); // "Hello, null"
```

Es muy frecuente utilizar los valores por defecto en conjunción con el destructuring:

```js
const logName = ({ name = "Unknown" }) => console.log(name);
logName({ age: 24 }); // "Unknown"
logName({ name: "Carl" }); // "Carl"
logName({}); // "Unknown"

// Pero ¿qué pasaría si llamo a la función sin argumento? ¿o con argumento null?

logName(); // [!] Si no inicializamos el parametro a {} esto daría TypeError.
// No se puede hacer destructuring sobre null o undefined

// Para cubrirnos con el caso de undefined podemos asignar valor por defecto a todo el argumento completo.
const logName = ({ name = "Unknown" } = {}) => console.log(name);

logName(); // Unknown. Ahora si!

// Sin embargo null seguría siendo el único caso problemático.
logName(null); // [!] Uncaught TypeError.
```
