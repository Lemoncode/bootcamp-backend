# Ejercicios TypeScript

## Ejercicio 1

Dados el siguiente snippet de código, crea la interfaz `Student` y úsala para sustituir los `unknown`:

```ts
const students: unknown = [
  {
    name: "Patrick Berry",
    age: 25,
    occupation: "Medical scientist",
  },
  {
    name: "Alice Garner",
    age: 34,
    occupation: "Media planner",
  },
];

const logStudent = ({ name, age }: unknown) => {
  console.log(`  - ${name}, ${age}`);
};

console.log("Students:");
users.forEach(logStudent);
```

## Ejercicio 2

Utilizando la interfaz `Student` del ejercicio anterior, crea la definición de `User`
de tal manera que pueda ser o `Student` o `Teacher`.
Aplica la definición de `User` donde sea requerido solventar los errores de tipos.

```ts
interface Teacher {
  name: string;
  age: number;
  subject: string;
}

type User = unknown;

const users: Teacher[] = [
  {
    name: "Luke Patterson",
    age: 32,
    occupation: "Internal auditor",
  },
  {
    name: "Jane Doe",
    age: 41,
    subject: "English",
  },
  {
    name: "Alexandra Morton",
    age: 35,
    occupation: "Conservation worker",
  },
  {
    name: "Bruce Willis",
    age: 39,
    subject: "Biology",
  },
];

const logUser = ({ name, age }: User) => {
  console.log(`  - ${name}, ${age}`);
};

users.forEach(logUser);
```

## Ejercicio 3

Con el resultado del ejercicio 2, sustituye la función `logUser` por la siguiente
y modifica el código aplicando las guardas que creas conveniente para corregir
los errores de compilación:

```ts
const logUser = (user: User) => {
  let extraInfo: string;
  if (user.occupation) {
    extraInfo = user.occupation;
  } else {
    extraInfo = user.subject;
  }
  console.log(`  - ${user.name}, ${user.age}, ${extraInfo}`);
};
```

> Extra: Crea dos funciones `isStudent` e `isTeacher` que apliquen la guarda y úsalas en la función `logPerson`.
> Aplica tipado completo en la función (argumentos y valor de retorno). Utiliza [is](https://www.typescriptlang.org/docs/handbook/2/narrowing.html#using-type-predicates).

## Ejercicio 4

Utilizando las misma interfaz de `Student`, de los ejercicios anteriores
arregla los errores de TypeScript para podamos pasar aquellos criterios que
necesitemos sin tener que pasar toda la información de un `Student`.
Arregla los subsiguientes errores que aparezcan al asignar tipo a `criteria`.

```ts
const students: Student[] = [
  {
    name: "Luke Patterson",
    age: 32,
    occupation: "Internal auditor",
  },
  {
    name: "Emily Coleman",
    age: 25,
    occupation: "English",
  },
  {
    name: "Alexandra Morton",
    age: 35,
    occupation: "Conservation worker",
  },
  {
    name: "Bruce Willis",
    age: 39,
    occupation: "Placement officer",
  },
];

const filterStudentsBy = (students: Student[], criteria: unknown): Student[] => {
  return students.filter((user) => {
    const criteriaKeys = Object.keys(criteria);
    return criteriaKeys.every((fieldName) => {
      return criteria[fieldName] === student[fieldName];
    });
  });
};

const logStudent = ({ name, occupation }: Student) => {
  console.log(`  - ${name}, ${occupation}`);
};

console.log("Students of age 35:");
filterStudentsBy(students, { age: 35 }).forEach(logStudent);
```

## Ejercicio 5

Mediante genéricos y tuplas, tipa de forma completa la función para solventar los
errores de compilación.

```ts
const swap = (arg1, arg2) => {
  return [arg2, arg1];
};

let age: number, occupation: string;

[occupation, age] = swap(39, "Placement officer");
console.log("Occupation: ", occupation);
console.log("Age: ", age);
```
