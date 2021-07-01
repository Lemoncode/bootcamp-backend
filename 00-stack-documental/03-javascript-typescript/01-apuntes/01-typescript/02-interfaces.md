# Interfaces

## Introducción

Una interfaz no es más que un contrato especificando las propiedades
que un objeto debe tener. Uno de los objetivos más básicos que tienen
las interfaces es que nos permiten dar nombres a las estructuras y
por lo tanto podemos reusarlos.

```ts
interface Coord {
  lat: number;
  lon: number;
}

const pos: Coord = {
  lat: 3.3112,
  lon: 5.1123,
};
```

En caso de no especificar todas las propiedades lanzará un error decompilación:

```ts
const pos1: Coord = {
  lat: 3.3112, // [ts] Property 'lon' is missing.
};
```

También si especificamos propiedades por exceso, lanzaría un error.

```ts
const pos2: Coord = {
  lat: 3.3112,
  lon: 5.1123,
  alt: 325,
};
```

## Modificadores: propiedades opcionales y read-only

TypeScript soporta propiedades opcionales mediante el operador `?`. Una
propiedad opcional no es más que aquella cuyo valor puede ser ser definido
o no (en este segundo caso sería undefined).

Además, podemos forzar a que ciertas propiedades sean de sólo lectura. Para
ello usaremos el operador `readonly` delante de la propiedad.
Con esto conseguimos establecer el valor de la propiedad a la hora
de crear el objeto denegando la posibilidad (a nivel de compilación)
de reasignar dicha propiedad.

Ejemplo:

```ts
interface Coord {
  readonly lat: number;
  readonly lon: number;
  alt?: number;
}

const pos: Coord = {
  lat: 3.3112,
  lon: 5.1123,
  // alt: 350, // Optional
};

pos.lat = 3.4567; // Cannot assign to 'lat' because it is a read-only property
```

## Anidado y extensión de interfaces

Podemos **anidar**, es decir, componer interfaces a partir de otros:

```ts
interface Address {
  zipCode: number;
  city: string;
  street: string;
}

interface Coord {
  lat: number;
  lon: number;
  address: Address;
}

const place: Coord = {
  lat: 3.3112,
  lon: 5.1123,
  address: {
    city: "Málaga",
    street: "Héroes de Sostoa",
    zipCode: 29002,
  },
};
```

También es frecuente utilizar la **extensión** de interfaces como método de composición. Es decir, una interfaz puede extender de otra para combinar propiedades:

```ts
interface Address {
  zipCode: number;
  city: string;
  street: string;
}

interface Coord {
  lat: number;
  lon: number;
}

interface Place extends Coord {
  address: Address;
}

const place: Place = {
  lat: 3.3112,
  lon: 5.1123,
  address: {
    city: "Málaga",
    street: "Héroes de Sostoa",
    zipCode: 29002,
  },
};
```

Incluso sería posible la extensión múltiple:

```ts
interface Place extends Coord, Address {}

const place: Place = {
  lat: 3.3112,
  lon: 5.1123,
  city: "Málaga",
  street: "Héroes de Sostoa",
  zipCode: 29002,
};
```

## Duck typing

TypeScript no tan estricto a la hora de tipar y no tiene en cuenta
semánticamente los tipos, sino que sigue una comparación estructural.
Vemos el siguiente ejemplo:

```ts
interface User {
  id: number;
  name: string;
}

const julia: User = {
  id: 31,
  name: "Julia",
};

interface Dog {
  id: number;
  name: string;
}

const laika: Dog = {
  id: 119,
  name: "Laika",
};

// Si declaramos la siguiente función para mostrar el nombre de un usuario:
function printUserName(user: User) {
  console.log(user.name);
}

// Vemos que no tenemos problemas a la hora de usarlo con un objeto de tipo User
printUserName(julia);

// Pero tampoco hay problemas a la hora de utilizarlo con un objeto de tipo Dog
printUserName(laika);
```

Vemos que `User` y `Dog` comparten la misma estructura. Si intentamos usarlo con
otra interfaz que no comparta sus mismos atributos dará un error de tipos.

```ts
interface Cat {
  name: string;
}

const garfield: Cat = { name: "Garfield" };

// Garfield no tiene "id: number" por lo que no compatible con el tipo que necesita
printUserName(garfield);
```

Como vemos si la interfaz no tiene las propiedades requeridas no podrá
ser usado por la función. Sin embargo esto no significa que la interfaz
pueda tener más propiedades

```ts
interface Fish {
  id: number;
  name: string;
  colors: string[];
}

const bob: Fish = { id: 319, name: "Bob", colors: ["white", "yellow"] };

// bob al tener "id: number" y "name: string" es compatible, pese a tener más propiedades de las requeridas
printUserName(bob);
```

Es decir, actualmente la función `printUserName` necesita un objeto que
estructuralmente tenga como mínimo las dos propiedades de `User`.
