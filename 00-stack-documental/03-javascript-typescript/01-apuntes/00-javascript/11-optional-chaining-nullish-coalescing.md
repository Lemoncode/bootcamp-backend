# Optional chaining

Supongamos el siguiente objeto:

```js
const user = {
  name: "Javi",
  // stats: {
  //   likes: 38,
  //   rt: 56,
  // }
  // friends: ["Santi", "Ana"],
  // greet: () => console.log("Hey there! Whats up");
};
```

Los accesos a propiedades inexistentes son seguros ya que devuelve `undefined`, pero en el momento en que esos accesos son en profundidad, o bien llamadas a funciones, dejan de ser seguros.

```js
console.log(user.stats); // Seguro -> 'undefined'
console.log(user.stats.like); // NO seguro -> TypeError: Cannot read property 'like' of undefined
```

La forma clásica de resolver este problema es usando chequeos _inline_, lo cual es engorroso:

```js
console.log(user && user.stats && user.stats.likes);
```

El operador _optional chaining_ proporciona una forma segura de acceso en profundidad a propiedades de objetos sin tener que validar su existencia explícitamente:

```js
console.log(user?.stats?.likes);
console.log(user?.friends[1]); // SyntaxError
console.log(user?.friends?.[1]); // Acceso seguro
console.log(user?.greet()); // SyntaxError
console.log(user?.greet?.()); // Acceso seguro
```

> IMPORTANTE: el operador optional chaining comprueba si una propiedad o referencia es _nullish_, es decir, `null` o `undefined`. En tal caso, cortocircuita devolviendo `undefined`, en caso contrario continúa.

# Nullish coalescing

El operador nullish coalescing es un operador lógico que devuelve el operando derecho cuando el izquierdo es _nullish_, es decir, `null` o `undefined`. Viene a mejorar la forma en que asignamos valores por defecto.

Hasta ahora era frecuente utilicar el operador lógico OR (`||`) para cortocircuitar valores por defecto, aunque es problemático:

```js
const quantity = 43;
console.log(quantity || "unknown"); // 43
```

```js
const quantity = null;
console.log(quantity || "unknown"); // unknown
```

¿Pero y si `quantity` es _falsy_?

```js
const quantity = 0;
console.log(quantity || "unknown"); // unknown
```

Puede suponer un problema ya que `0` es una cantidad legítima, no debería retornar `unknown`.

Con el nullish coalescing comprobamos que el valor sea _nullish_ en lugar de _falsy_:

```js
console.log(quantity ?? "unknown");
```

Es común combinar _nullish coalescing_ con _optional chaining_ para hacer código robutso, con accesos seguros y valores por defecto en caso de no existir. Siguiendo el ejemplo anterior:

```js
console.log(user?.stats?.likes ?? "Not available");
```
