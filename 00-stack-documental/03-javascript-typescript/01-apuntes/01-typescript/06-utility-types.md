# Tipos genéricos de utilidades

En TypeScript existen una serie de tipos mapeados genéricos que tenemos a nuestra disposición

## ReadOnly

Convierte todas las propiedades de un interfaz en solo lectura:

```ts
interface State {
  username: string;
  password: string;
}

type ROState = Readonly<State>;

// También aplicable a arrays:
type ROArray<T> = Readonly<Array<T>>;
// De hecho existe el tipo ReadonlyArray<T>
```

Un caso muy útil de Readonly se aplica para garantizar que una
implementación no va a mutar un objeto determinado. Así, el consumidor
de que los parámetros de entrada no serán mutados. Pongamos un
de dicha implementación (una función por ejemplo) tiene garantías
ejemplo con arrays, donde se ve más claro, aunque lo mismo serviría
para objetos:

```ts
const sampleArray = [1, 2, 3];

const tailMutable = <T>(array: T[]): T[] => (array.shift(), array);

const tailImmutable = <T>(array: Readonly<T[]>): T[] => {
  const [, ...tail] = array || [];
  return tail;
};

console.log(sampleArray, tailMutable(sampleArray));
console.log(sampleArray, tailImmutable(sampleArray));
```

Su definición es:

```ts
type Readonly<T> = {
  readonly [P in keyof T]: T[P];
};
```

## Partial

Convierte en opcionales las propiedades de una interfaz, en la práctica
esto permite usar implementaciones parciales de un tipo o interfaz:

```ts
interface Person {
  name: string;
  age: number;
}

type PartialPerson = Partial<Person>;
```

Veamos un caso práctico:

```ts
// -- Caso Práctico --
const createState = <T extends object>(initialState: T) => {
  let state: T = initialState;

  return {
    setState: (partialState: Partial<T>): T => (state = { ...state, ...partialState }),
  };
};

const { setState } = createState({
  username: "b4dc4t",
  avatar: "cat.png",
  posts: 18,
  premium: false,
});

console.log(setState({ posts: 19, premium: true }));
```

Su definición es:

```ts
type Partial<T> = {
  [P in keyof T]?: T[P];
};
```

## Required

La contraparte de Partial, convierte en requeridas las propiedades
de una interfaz:

<!-- prettier-ignore -->
```ts
interface Coord {
  x: number;
  y: number;
  z?: number;
}

type Coord3D = Required<Coord>;

const coord: Coord3D = { // [ts] Property 'z' is missing
  x: 0,
  y: 0,
};
```

Su definición es la siguiente:

```ts
type Required<T> = {
  [P in keyof T]-?: T[P];
};
```

## Exclude y Extract

`Exclude` excluye de la primera unión los tipos que tiene en común con la
segunda. Por tanto calcula la diferencia matemática.

`Extract` extrae de la primera unión los tipos que tiene en común con la
segunda. Por tanto calcula la intersección matemática.

```ts
type WeekDay = "lun" | "mar" | "mie" | "jue" | "vie" | "sab" | "dom";
type WorkDay = Exclude<WeekDay, "sab" | "dom">;
type Weekend = Extract<WeekDay, "sab" | "dom" | "x">;
```

Sus definiciones son las siguientes:

```ts
type Exclude<T, U> = T extends U ? never : T;
type Extract<T, U> = T extends U ? T : never;
```

## Pick y Omit

`Pick` permite generar un sub-interfaz, a partir de un interfaz ya
existente, escogiendo las propiedades que queremos del original y
tipándolas de igual forma. En definitiva, extrae un subconjunto de
propiedades (y sus tipos) de una interfaz para generar otra distinta.

`Omit` es el opuesto de `Pick`, nos permite generar un interfaz a partir de
otro pero en este caso eliminando las propiedades que no deseamos. Es
decir, genera una nueva interfaz excluyendo propiedades de la original.

```ts
interface EmployeeSummary {
  name: string;
  id: string;
  age: number;
  phone: number;
  married?: boolean;
}
type EmployeeID = Pick<EmployeeSummary, "id" | "name">; // Check intellisense!
type EmployeeDetails = Omit<EmployeeSummary, keyof EmployeeID>; // Check intellisense!
```

Sus definiciones son:

```ts
type Pick<T, K extends keyof T> = {
  [P in K]: T[P];
};
type Omit<T, K extends keyof any> = Pick<T, Exclude<keyof T, K>>;
```

## Record

Es un tipo bastante útil para armar objetos desde cero a partir de un
conjunto de claves definidas a las que asignamos un tipado concreto.

```ts
type Sizes = "small" | "medium" | "large";
type EurSizes = Record<Sizes, string>;
type UkSizes = Record<Sizes, number>;

const eurSizes: EurSizes = { small: "s", medium: "m", large: "l" };
const ukSizes: UkSizes = { small: 8, medium: 10, large: 12 };
```

Su definición es:

```ts
type Record<K extends keyof any, T> = {
  [P in K]: T;
};
```

## NonNullable

Genera un tipo nuevo a partir de otro, previniendo
que su valor pueda ser null o undefined.

Tiene sentido en un entorno estricto `strictNullChecks: true` de lo contrario
el compilador siempre permitirá valores a null/undefined para cualquier
tipo.

```ts
// -- Caso Práctico --

// Un 'draft' puede ser usado tanto en un formulario de edición como en uno de inserción
interface BookingDraft {
  id: number | null; // 'number' para edición, 'null' para inserción
  price: number;
  room: "standard" | "superior";
  prepaid: boolean;
}

// Declaramos un helper que nos permita seleccionar qué propiedades no pueden ser 'null'
type NonNullableKeys<O, K extends keyof O> = {
  [P in keyof O]: P extends K ? NonNullable<O[P]> : O[P];
};

type Booking = NonNullableKeys<BookingDraft, "id">;
```

## ReturnType

Permite inferir el tipo de dato que devuelve una función.

```ts
interface UserID {
  name: string;
  id: number;
}

const addTimestamp = (user: UserID, useLocale: boolean = false) => ({
  ...user,
  timestamp: useLocale ? Date().toLocaleString() : Date.now(),
});

type UserWithTimestamp = ReturnType<typeof addTimestamp>; // Check intellisense!
```

## Parameters

Permite inferir el tipado de los argumentos de entrada de
una función en formato tupla.

```ts
const addTimestamp = (user: UserID, useLocale: boolean = false) => ({
  ...user,
  timestamp: useLocale ? Date().toLocaleString() : Date.now(),
});

type Params = Parameters<typeof addTimestamp>; // Check intellisense!
```
