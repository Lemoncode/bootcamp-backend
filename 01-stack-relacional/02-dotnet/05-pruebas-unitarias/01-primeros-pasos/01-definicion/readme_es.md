# Qué son los tests unitarios

Vamos a ver qué es un test unitario cuáles son sus características y ventajas y qué herramientas nos ofrece Visual Studio para trabajar con ellos. En primer lugar, vamos a definir qué es un test unitario.

Un test unitario es un método automático que prueba una parte muy pequeña de nuestro sistema, un método de una clase con una responsabilidad única y aislada. El test unitario asegura que con una llamada concreta a un método, siempre obtendremos el resultado esperado. Podemos destacar las siguientes caracteristicas:

- ***Unitaria***. Prueba solamente pequeñas cantidades de código con una responsabilidad muy concreta. No prueba un sistema grande o varios métodos a la vez. Esos serían los tests de integración.
- ***Independiente***. Un test unitario no depende de otros sistemas si no, volvemos a caer en el test de integración.
- Prueba solo ***métodos públicos*** de nuestro sistema. Nunca probará métodos privados, en primer lugar porque no podría alcanzarlos y en segundo lugar, porque los métodos públicos ya llaman de por sí a los métodos privados.
- ***Automatizable***. La prueba no requiere de intervención manual, de hecho, podemos incluso configurar Visual Studio para que ejecute nuestros tests de manera automática cada vez que se compile nuestro proyecto.
- ***Repetible y predecible***. Cada llamada concreta debe siempre devolver el mismo resultado.
- ***Rápido y facil*** de codificar. Hay una creencia muy extendida entre los desarrolladores y es que un test unitario es lento, es pesado, enlentece el desarrollo lo hace menos ágil y nada más lejos de la realidad. Un test unitario debe ser fácil de codificar y no debería de tomarnos más de cinco minutos. Con las ventajas que los tests unitarios nos ofrecen realmente merece la pena codificarlos mientras hacemos nuestro desarrollo.

Los tests unitarios tienen diversas ventajas:

- Ayudan a la ***refactorización***, al poder probar nuestro proyecto después de ciertos cambios, siempre podremos reejecutar los tests cuando refactoricemos y poder ver si lo que hemos hecho ha roto algo dentro de nuestro código.
- ***Simplifican la integración*** debido a que probamos piezas pequeñas cuando lancemos los tests y estos pasen esas piezas pequeñas seguro que están funcionando correctamente por lo cual, al integrarlas en un sistema más grande garantizaremos que esa integración es correcta.
- Sirven como ***documentación*** de nuestro código. Los tests unitarios están haciendo llamadas a nuestro método de forma correcta con resultados muy concretos y esperados, por lo cual, además de poder ver los distintos flujos de ejecución que estamos cubriendo en esos tests también vemos la forma de llamar a todos los métodos testeados.
- Hay ***menos errores***, y mucho más fáciles de localizar. Si realmente estamos cubriendo bastante código con nuestros tests los errores serán muy pequeños, puesto que, por lanzar los tests, estamos viendo si lo que esperamos se está cumpliendo o no.
- Mejor diseño, ***uso de TDD***. El TDD, Test Driven Development, es un patrón que invierte el orden de codificación primero se codifican los casos de uso en test y a continuación, se codifica el código para que cumpla con esos tests. Tests unitarios es una forma muy sencilla de cumplir con este patrón ya que nos permite hacer exactamente esto, codificar los casos de uso, los tests y a continuación, el código pasar los tests y ver si ese código cumple con todos los casos de uso que hemos definido.

Hay bastantes herramientas de Visual Studio que nos van a permitir trabajar con tests unitarios. Visual Studio tiene integración con tres motores de test:

- ***MSTest***, que es el de Microsoft.
- ***NUnit***
- ***XUnit***

Con cualquiera de estos tres podremos utilizar las herramientas para los test de *Visual Studio*:

*Test Explorer* es la interface muy intuitiva que tiene *Visual Studio* para trabajar con nuestros tests unitarios. Podemos organizarlos por categorías, podemos organizarlos por *playlist*, ejecutarlos de una forma muy rápida, ver si pasan o no pasan podemos ir directamente al método que queramos si no ha pasado y una serie de cosas que nos van a hacer la vida mucho más fácil a la hora de codificar y probar nuestros tests unitarios.

*Intelli Tests*, creación automática de test. Esta es una característica muy interesante de *Visual Studio Enterprise* que nos permite crear tests unitarios automáticos que van a recorrer de forma exhaustiva todos los flujos de ejecución de nuestro código y así poder ver si nos hemos dejado algo por codificar que pueda producir un error.

*Live tests*. Esta función nos permite, si tenemos un test hecho para un trozo de código, ver en tiempo real cuando estamos cambiando nuestro código si esos cambios están afectando a esos tests, directamente nos marca en color si las líneas que hemos cambiado hacen que los tests que las cubren estén fallando.

*Code coverage*. Esta función nos permite ver exactamente cuánto código estamos cubriendo con nuestros tests. Al ejecutarlo, se ejecutan todos los tests, se recorren todos los flujos y nos dice exactamente cuánto código está cubierto por esos tests. Podemos navegar por las clases, ver qué porcentaje de cada uno de los métodos está siendo cubierto, y así corregir los puntos débiles que tenemos sin cubrir en nuestro código.
