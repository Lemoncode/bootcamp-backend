# Introducción a promesas

Para dominar JavaScript es imprescindible tener unas buenas nociones
de asincronía y conocer el "Event Loop" que implementa el lenguaje
como solución para gestionar eventos y llamadas asíncronas.
Recomendamos encarecidamente la lectura de la
[siguiente guía](https://lemoncode.net/lemoncode-blog/2018/1/29/javascript-asincrono).

Una llamada asíncrona es aquella donde la tarea asociada se ejecuta
fuera del contexto de nuestra aplicación,y por tanto nuestra aplicación
no consume recursos (CPU). A esto se le conoce como operaciones de
entrada/salida (I/O Operations). Pensad en un acceso a disco o en una
consulta a servidor.
Además, en las llamadas asíncronas, la respuesta se notifica a nuestro
programa, evitando que quede bloqueado a la espera de una respuesta.
Es decir, nuestro programa lanza la llamada asíncrona, continúa su
ejecución y en algún momento será notificado con la respuesta a dicha
llamada.

## Callbacks

El patrón mas sencillo para manejar llamadas asíncronas son los
CALLBACKS, es decir, una función que se pasa como argumento de otra.
La finalidad del callback es registrar el código que debe ser ejecutado
una vez tengamos nuestra respuesta.

La función `setTimeout` es una de las funciones asíncronas más sencillas que hay:
postpone la ejecución de un callback, como mínimo, a X segundos después.

```js
const callback = () => console.log("Hello World! con retardo");
setTimeout(callback, 1000);
```

Al ser asíncrona, nuestra aplicación sigue corriendo:

```js
const callback = () => console.log("Hello World! con retardo");
setTimeout(callback, 1000);

console.log("No estoy bloqueada, puedo ejecutar código");
```

Resultado por consola:

```
No estoy bloqueada, puedo ejecutar código
Hello World! con retardo
```

Veamos un ejemplo más elaborado de uso del patrón callback donde pedimos datos a
un servicio que puede tardar en devolvernos los datos:

```js
// Objeto simulando una API que se conecta a una base de datos.
const db = {
  getUsers(callback) {
    setTimeout(() => {
      // Invocamos al callback con los datos
      callback([{ name: "fjcalzado" }, { name: "crsanti" }]);

      // Esperamos entre 1 y 3 segundos para responder
    }, Math.random() * 2_000 + 1_000);
  },
};

// Usamos la API pasándole un callback que recibirá los datos
db.getUsers(users => {
  console.log("Received users", users);
});
```

## Promesas

### Introducción

Una promesa es un objeto que representa el resultado de una operación
asíncrona. Este resultado podría estar disponible ahora o en el futuro.
Una promesa puede tener los siguientes estados:

- A la espera de respuesta -> PENDING
- Finalizada -> SETTLED. En este caso, puede terminar con 2 estados:
  - Operación completada con éxito -> FULFILLED or RESOLVED
  - Operación rechazada con fallo o error -> REJECTED

Las promesas se basan en callbacks pero son una evolución de éstos, una
mejora, que añade azúcar sintáctico para un mejor manejo.

_Analogía de la pizza y el beeper_

### Creando promesas

Una promesa se crea instanciando un nuevo objeto `Promise`. En el momento
de la creación, en el constructor, debemos especificar un callback que
contenga la carga de la promesa, aquello que la promesa debe hacer.
Este callback nos provee de dos argumentos: `resolveCallback` y
`rejectCallback`. Te suenan, ¿verdad? Son los dos mismos callbacks
que se registrarán al consumir la promesa. De este modo, depende de ti
como desarrollador llamar a `resolveCallback` y `rejectCallback` cuando sea
necesario para señalizar que la promesa ha sido completada con éxito
o con fallo.

Modifiquemos el objeto `db` para añadir una versión de `getUsers` utilizando el patrón de promesas:

```js
const db = {
  getUsers(callback) {
    setTimeout(() => {
      // Invocamos al callback con los datos
      callback([{ name: "fjcalzado" }, { name: "crsanti" }]);

      // Esperamos entre 1 y 3 segundos para responder
    }, Math.random() * 2_000 + 1_000);
  },
  getUsersPromisified() {
    // Devolvemos una promesa que se resolverá eventualmente
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        try {
          // throw new Error('Connection closed');

          // Resolvemos la promesa con los datos
          resolve([{ name: "fjcalzado" }, { name: "crsanti" }]);
        } catch (error) {
          reject(error);
        }
        // Esperamos entre 1 y 3 segundos para responder
      }, Math.random() * 2_000 + 1_000);
    });
  },
  // Alternativamente, podriamos haberlo implementado llamando a su vez
  // a getUsers
  getUsersPromisifiedAlt() {
    return new Promise((resolve, reject) => {
      db.getUsers(resolve);
    });
  },
};
```

### Consumiendo promesas

Cuando llamamos a una función asíncrona implementada con Promesas, nos
devolverá inmediatamente un objeto promesa como garantía de que la
operación asíncrona se ha puesto en marcha y finalizará en algún momento,
ya sea con éxito o con fallo.
Una vez que tengamos el objeto promesa en nuestro poder, lo usamos para
registrar 2 callbacks: uno para indicar 'que se debe hacer en caso de
que todo vaya bien' (resolución de la promesa o resolve) y otro para
indicar 'que hacer en caso de fallo' (rechazo de la promesa o reject).

<!-- prettier-ignore -->
```js
// La invocación de `getUsersPromisified` devolverá una promesa.
const promise = db.getUsersPromisified();

// Registramos los callbacks
promise
  .then((users) => console.log("Received users", users))
  .catch((error) => console.log("Received error", error.message));
```

> Si los callbacks de `then` y `catch` devuelven otras promesas éstas
> serán resueltas y sus resultados pasarán a los siguientes callbacks `then` o `catch` dependiendo de cómo se haya resuelto la promesa.

También podemos registrar el callback de un `catch` como segundo argumento de `then` si queremos manejar el error específicamente en ese momento (sería equivalente a tener múltiples `try-catch` en vez de uno solo).

```ts
promise.then(
  users => console.log("Received users", users),
  error => console.log("Received error", error.message)
);
```

Además podemos utilizar el método `finally` para ejecutar una función cuando la promesa se complete independientemente de si fue rechazada o no.

<!-- prettier-ignore -->
```ts
promise
  .then((users) => console.log("Received users", users))
  .catch((error) => console.log("Received error", error.message))
  .finally(() => console.log('Promise settled'))
```

> Los métodos `then`, `catch` y `finally` devuelven nuevas instancias de promesas.

## Funciones asíncronas: async / await

Las promesas, al igual que los callbacks, pueden llegar a ser tediosas
cuando se anidan y se requieren más y más .then(). Async / Await son 2
palabras clave que surgieron para simpificar el manejo de las promesas.
Son azúcar sinctáctico para reducir el anidamiento y manejar código
asíncrono como si de código síncrono se tratara.

Vamos a modificar el ejemplo de `db` para incorporar un método `async`:

```js
const db = {
  getUsers(callback) {
    setTimeout(() => {
      // Invocamos al callback con los datos
      callback([{ name: "fjcalzado" }, { name: "crsanti" }]);

      // Esperamos entre 1 y 3 segundos para responder
    }, Math.random() * 2_000 + 1_000);
  },
  getUsersPromisified() {
    return new Promise((resolve, reject) => {
      db.getUsers(resolve);
    });
  },
  async getUsersAsync() {
    // throw new Error('Connection closed');
    const users = await db.getUsersPromisified();
    return users;
  },
};
```

Consumiremos la función asíncrona de la misma manera que con `getUsersPromisified`:

<!-- prettier-ignore -->
```js
// La invocación de `getUsersAsync` devolverá una promesa.
const promise = db.getUsersAsync();

// Registramos los callbacks
promise
  .then((users) => console.log("Received users", users))
  .catch((error) => console.log("Received error", error.message));
```
