# Tipos avanzados

Hasta ahora hemos visto la base de Typescript sobre la que se sustenta
todo el chequeo de tipos. Pero la verdadera potencia llega con los tipos
avanzado.

## Alias

Un alias no es más que un nuevo nombre para un tipo, sea cual sea,
tipos primitivos, interfaces, funciones, uniones, etc:
Es muy util para REUSAR tipos cuya definición es más compleja o verbosa
de forma fácil y eficiente, abstrayéndola a un nombre simple, sin tener
que repetirla una y otra vez.

```ts
// Alias de un tipo primitivo
type Id = number;
const number: Id = 123;

// Alias de un tipo de función
type FunctionVoid = () => void;
const noop: FunctionVoid = () => {};

// Alias de una estructura con ciertas propiedades
type Box<V> = {
  value: V;
};
const box: Box<boolean> = {
  value: true,
};
```

## Intersección

Con la intersección podemos combinar múltiples tipos en uno solo. Sería
equivalente al operador AND lógico pero con tipos.
Es muy util para hacer composiciones:

```ts
type Merged = { a: string } & { b: string };
const ab: Merged = { a: "A", b: "B" };
```

En caso de colisión de tipos primitivos el tipado será `never`:

```ts
type MergedCollision = { a: string } & { a: number };
const abc: MergedCollision = { a: 1 }; // [ts] Type 'number' is not assignable to type 'never'
```

Veamos un ejemplo práctico de uso:

```ts
const compose = <A, B>(a: A, b: B): A & B => ({ ...a, ...b });

const cat = compose({ type: "feline" }, { skill: "hunting" });
const pigeon = compose({ wings: true }, { type: "bird" });

console.log(cat.type);
console.log(pigeon.skill); // TS error: Property 'skill' is missing.
```

## Unión

Siguiendo la analogía anterior, la unión de tipos sería entendida como
el operador OR. La unión de tipos es muy util para indicar que una
determinada entidad podrá ser de un tipo u otro, ámbos válidos.

```ts
type Id = number | string;
const id1: Id = 123;
const id2: Id = "123";

// Unión en argumentos de una función
const showId = (id: number | string) => {
  console.log("Id", id);
};
```

Con uniones más complejas es probable que lleguemos a escenarios donde tengamos
que comprobar de que tipo concreto es un determinado argumento
recibido, de entre todos los posibles tipos de su unión. Es por ello que existen
operadores para desambiguar los tipos.

## Guardas

Existen ciertos operadores nativos de JavaScript que son capaces de acortar
los tipos de una unión

### Guardas con el operador `in`.

```ts
interface Point {
  x: number;
  y: number;
}

interface Line {
  from: Point;
  to: Point;
}

interface Polygon {
  points: Point[];
}

type Feature = Point | Line | Polygon;

const point: Feature = {
  x: 10,
  y: -8,
};

const handleFeature = (feature: Feature) => {
  if ("points" in feature) {
    console.log("Feature is polygon", feature.points);
  } else if ("from" in feature) {
    console.log("Feature is line", feature.from, feature.to);
  } else {
    console.log("Feature is point", feature.x, feature.y);
  }
};
```

## Guardas con el operador `typeof`:

```ts
const double = (arg: string | any[]) => {
  if (typeof arg === "string") {
    // TypeScript entiende que arg es de tipo `string`
    return arg + arg;
  } else {
    // TypeScript entiende que arg es de tipo `any[]`
    return arg.concat(arg);
  }
};

console.log(double("123")); // "123123"
console.log(double([1, 2, 3])); // [1, 2, 3, 1, 2, 3]
```

## Guardas con el operador `instanceof`:

```ts
class Car {
  fuel: number;
  constructor(fuel: number) {
    this.fuel = fuel;
  }
}

class Bike {
  hasBasket: boolean;
  constructor(hasBasket: boolean) {
    this.hasBasket = hasBasket;
  }
}

type Vehicle = Car | Bike;

const handleVehicle = (vehicle: Vehicle) => {
  if (vehicle instanceof Car) {
    console.log("Car fuel:", vehicle.fuel);
  } else {
    console.log(`Bike with${vehicle.hasBasket ? " " : "no "} basket`);
  }
};

handleVehicle(new Car(56));
```

## Tipos literales

Muy útil para hacer que un tipo solo pueda tomar determinados
valores literales, es decir, limitar el conjunto de posibles valores
a un subconjunto finito. Se emplea habitualmente junto con la unión:

```ts
type LabourDay = "monday" | "tuesday" | "wednesday" | "thursday" | "friday";
const day: LabourDay = "sunday"; // TS error: '"sunday"' is not assignable to type 'LabourDay'

// También es aplicable a números literales:
const throwDice = (): 1 | 2 | 3 | 4 | 5 | 6 => {
  return 6; // Dado trucado MUAHAHAHAHA.
};
```

## El operador keyof

Operador muy util para extraer las propiedades de un interfaz como
posibles literales de un tipo concreto:

```ts
interface Week {
  monday: string;
  tuesday: string;
  wednesday: string;
  thursday: string;
  friday: string;
  saturday: string;
  sunday: string;
}
type Day = keyof Week;
const day1: Day = "tuesday";
const day2: Day = "martes"; // [ts] Type '"martes"' is not assignable to type 'keyof Week'
```

Un caso práctico de uso:

```ts
const showProps = <T>(obj: T, ...keys: (keyof T)[]): void => {
  keys.forEach((key) => console.log(obj[key]));
};

const dev = {
  type: "frontend",
  languages: ["js", "css", "html"],
  senior: true,
};
showProps(dev, "type", "languages"); // Check intellisense!;
```

## Tipos mapeados (Mapped types)

Nos permiten crear nuevos alias a partir de otro tipo.

Pongamos la siguiente interfaz:

```ts
interface ProductItem {
  name: string;
  price: number;
}
```

Podemos acceder al tipo de una propiedad usando los corchetes:

```ts
type ProductName = ProductItem["name"]; // string
```

Vamos a crear un nuevo tipo a partir de ProductItem que tenga todas sus propiedades pero el tipo sea `boolean`:

```ts
type ProductItemFlags = {
  [Key in keyof ProductItem]: boolean;
};
const productItemFlags: ProductItemFlags = {
  name: true,
  price: false,
};
```

Los tipos mapeados son muy útiles para ser usados como generadores de tipos que son capaces de añadir o eliminar propiedades o alterar los modificades de propiedades existentes. Veremos algunos de ellos en [la siguiente sección](./06-utility-types.md).

## Tipos condicionales

Permite mapear a diferentes tipos comprobando el valor de otro.
En la practica es equivalente a poder usar ternarios para tipos.

```ts
type DarkColors = "black" | "grey";
type LightColors = "white" | "yellow" | "pink";

type Status = "sad" | "happy";

type Palette<P extends Status> = P extends "sad" ? DarkColors : LightColors;

const palette: Palette<"sad"> = "black"; // Only black or grey allowed.
```
