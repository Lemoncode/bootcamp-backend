# Clases

## Introducción

Las clases son objetos que actúan como plantilla para crear otros objetos. Como ya hemos visto anteriormente, un objeto es una estructura de datos que encapsula un conjunto de datos. Las clases son azúcar sintáctico de una de las características más complejas y potentes de JavaScript: el modelo prototípico.

## Sintaxis

Para crear una instancia nueva necesitamos una clase y el operador `new`. El `constructor` es una función especial que se ejecutará durante la creación de la instancia.

```js
class Vehicle {
  constructor(wheels) {
    this.wheel = wheels;
    this.kms = 0;
  }
}
const car = new Vehicle(4);
const bicicle = new Vehicle(2);
console.log(car1); // Vehicle{wheels: 4, kms: 0}
console.log(bicicle); // Vehicle{wheels: 2, kms: 0}
```

Dentro del constructor `this` será una variable que referencia a la instancia de la clase que estamos creando. Cada propiedad que asignemos a `this` será añadida como propiedad de la instancia.

Cada instancia mantiene sus propiedades de forma encapsulada sin alterar a otras instancias. Se pueden acceder a ellas igual que con cualquier objeto.

```js
console.log(car.kms); // 0
console.log(bicicle.kms); // 0
car.kms = 100;
console.log(car.kms); // 100
console.log(bicicle.kms); // 0
```

## Métodos

Podemos definir métodos en la clase que sean accesible por las instancias sin tener que añadirlos como propiedad de la instancia. Esto es eficiente porque se crean una única vez y no una vez por cada instancia creada. El lugar donde se guardan estas propiedades es en el prototipo de la clase.

```js
class Vehicle {
  constructor(wheels) {
    this.wheel = wheels;
    this.kms = 0;
  }

  drive(kms) {
    this.kms += kms;
    console.log("Driving " + kms + "kms...");
  }

  showKms() {
    console.log("Total Kms: " + this.kms);
  }
}

const car = new Vehicle(4);
car.drive(100); // Driving 100 kms...
car.showKms(); // Total Kms: 100
```

## Herencia

Podemos hacer que una clase extienda de otra, aplicando sus constructores y permitiendo acceder sus métodos, similar a otros lenguajes, aunque internamente es más complejo ya que JavaScript es un lenguaje orientado a prototipos y no a objetos.

```js
class Vehicle {
  constructor(wheels) {
    this.wheel = wheels;
    this.kms = 0;
  }

  drive(kms) {
    this.kms += kms;
    console.log("Driving " + kms + "kms...");
  }

  showKms() {
    console.log("Total Kms: " + this.kms);
  }
}

class Taxi extends Vehicle {
  constructor() {
    super(4);
    this.isOccupied = false;
  }

  service() {
    this.isOccupied = true;
  }
}

const taxi = new Taxi();
console.log(taxi); // Taxi{wheels: 4, kms: 0, isOccupied: false}
taxi.service();
console.log(taxi.isOccupied); // true
console.log(taxi.wheel); // 4
taxi.drive(100); // Driving 100 kms
```

> Cuando definimos la clase sólo podemos heredar de una sola clase, aunque esto no impide que la clase de la que partimos extienda de otra.

## Aseveración de instancia

Podemos comprobar si una instancia es realmente instancia de una clase con el operador `instanceof`.

```js
console.log(taxi instanceof Taxi); // true
console.log(taxi instanceof Vehicle); // true
console.log(car instanceof Taxi); // false
console.log(taxi instanceof Object); // true
console.log(car instanceof Object); // true
```

También podemos acceder a la clase constructora, referenciada en la propiedad `constructor` de la clase

```js
console.log(taxi.constructor === Taxi); // true
console.log(car.constructor === Vehicle); // true
```
