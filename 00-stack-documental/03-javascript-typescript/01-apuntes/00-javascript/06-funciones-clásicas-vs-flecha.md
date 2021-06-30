# Funciones

Las funciones son parte fundamental de cualquier lenguaje: permiten ejecutar un bloque de código (una secuencia de instrucciones), permitiendo la entrada de argumentos y devolviendo un valor de retorno.

En javascript tenemos 2 formas de declarar funciones: mediante las funciones clásicas o bien a través de las funciones flecha.

## Funciones clásicas

Las declaramos con la palabra clave `function`, seguido del nombre, la lista de argumentos y el cuerpo de la función. A esto se le llama `function declaration`:

```js
function toUpper(text) {
  return text.toUpperCase();
}

console.log(toUpper("hello world"));
```

Podemos manejar funciones y referirnos a ellas asignando una `function expression` a una variable:

```js
const toUpper = function (text) {
  return text.toUpperCase();
};

console.log(toUpper("hello world"));
```

## Funciones flecha

También conocidas como 'arrow functions', 'fat arrow' o 'lambdas'. Tienen una sintaxis más reducida y expresiva:

<!-- prettier-ignore -->
```js
const toUpper = (text) => {
  return text.toUpperCase();
};
```

Se pueden compactar todavía más cuando su cuerpo solo consta de una sentencia de retorno. Además, los paréntesis de la lista de argumentos se pueden omitir cuando sólo existe 1 único argumento:

```js
const toUpper = text => text.toUpperCase();
```

La excepción a la regla anterior es el caso en que devuelve un objeto literal, entonces las llaves de objeto podrían confundirse con el cuerpo de función, por eso hay que añadir paréntesis:

```js
const toObject = (name, surname, age) => {
  return { name, surname, age };
};

const toObjectCompact = (name, surname, age) => ({ name, surname, age });
```

## Ciudadanos de primer orden

Las funciones son ciudadanos de primer orden en javascript, lo que significa que pueden ser pasadas como argumentos de otra función (_callbacks_), o devueltas como valor de retorno:

```js
const toUpper = text => text.toUpperCase();
const toLower = text => text.toLowerCase();

function logWithDecorator(message, decoratorCallback) {
  console.log(decoratorCallback(message));
}

logWithDecorator("hello", toUpper);
logWithDecorator("WORLD", toLower);
```

## Argumentos

En javascript no estamos obligados a declarar todos y cada uno de los argumentos que vamos a utilizar. En las **funciones clásicas** podemos hacer uso de la palabra clave `arguments` para acceder a todos los argumentos que se han pasado en la llamada:

```js
function sum() {
  let total = 0;
  for (const num of arguments) {
    total += num;
  }
  return total;
}

console.log(sum(1, 2, 3)); // 6;
```

Sin embargo, `arguments` no está disponible para las funciones flecha. No obstante, veremos que se puede suplir un comportamiento similar gracias a operadores como `rest` y `spread`.

### ¿Porqué 2 tipos de funciones?

¿Porqué existe otra forma de expresar funciones en Javascript a través de las _arrow functions_? ¿Es mera estética? No. Aunque queda fuera del ámbito de este bootcamp, existe una diferencia fundamental entre las funciones clásicas y las funciones flecha, que justifica la existencia de ambas. Esta diferencia tiene que ver con la palabra clave `this`, palabra reservada en el ámbito de una función, que se emplea como puntero hacia el contexto asociado a dicha función. Este contexto será el contexto de ejecución en las funciones clásicas (¿qué contexto ha llamado a la función (caller)?) mientras que en las funciones flecha apuntará al contexto léxico (¿en qué contexto ha sido declarada la función?). Esto provoca un comportamiento diferenciado de la palabra `this`, resultándonos más adecuado el uso de uno u otro tipo de función en determinados escenarios.
