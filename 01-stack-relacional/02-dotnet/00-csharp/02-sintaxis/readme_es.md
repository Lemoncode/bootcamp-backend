# Sintaxis

En este apartado vamos a ver los conceptos básicos y la sintaxis de c#. Se va a abordar los siguientes apartados:

- Variables
- Operadores
- Expresiones
- Bucles
- Condicionales

Antes de empezar, vamos a definir que es un _Identificador_. Usaremos los identificadores para poder hacer referencia a los elementos que vamos creando (Namespaces, clases, métodos, variables, constantes...)

Cada tipo de elementos tendrá asociada una convención (las veremos más adelante) pero por regla general deben cumplir estas condiciones:

- Sólo se pueden utilizar letras (minúsculas y mayúsculas), números y guion bajo.
- Deben comenzar por una letra o un guion bajo.
- No se deben utilizar palabras claves reservadas.

## Variables

C# es un lenguaje fuertemente tipado, por tanto, todas las variables que declaremos tendrán que asociarse a un tipo de datos. Disponemos de los siguientes tipos de datos:

- Tipos integrados (Primitivos):
  - Enteros:
    - Con signos (sbyte, short, int, long)
    - Sin signos (byte, ushort, unint, ulong)
  - Reales (float, double, decimal)
  - Boolean (true, false)
  - utf16 (char)
- Tipos personalizados
  - Estructurados
  - Enumerados
  - Record
  - Clases

Una variable no es más que un espacio de memoria que reservamos para almacenar un dato. El tamaño de este espacio de memoria está definido por el tipo de datos que le demos a la variable. Vamos a ver los tipos de datos más usados y cuáles son sus tamaños en bits:

![Resumen de tipo de datos](./content/type_summary.png)

Fijándonos en el cuadro, si definimos una variable de tipo _float_, estamos reservando 32 bits para almacenarla, o si definimos una variable de tipo _char_ reservaríamos 16 bits.

Pero no nos preocupemos "mucho" por el espacio que vamos reservando de memoria, porque .net se encargará de ir eliminando de la memoria las variables que no vayamos a usar más.

Bueno, pues ahora vamos a declarar nuestras primeras variables. Para ello vamos a abrir el proyecto de _hola mundo_ que hemos creado en el apartado anterior e introducimos el siguiente código:

```diff
-  var a = 1;
-  a = MultiploPI(a);
-  Console.WriteLine(a);

    +  int entero1 = 10;
+  int entero2 = -10;
+  long numero_grande = 6546546950;
+  float numero_float1 = -11.5f; //Hay que poner la f porque por defecto C# trata los numeros decimales como doubles
+  float numero_float2 = float.MaxValue;
+  double numero_double = double.MaxValue;
+  decimal numero_decimal = decimal.MaxValue;

+  char caracter = '*';
+  string cadena = "Hola lemoncoders!";
+  bool verdadero = true;
+  bool falso = false;

+  Console.WriteLine(entero1);
+  Console.WriteLine(entero2);
+  Console.WriteLine(numero_grande);
+  Console.WriteLine(numero_float1);
+  Console.WriteLine(numero_float2);
+  Console.WriteLine(numero_double);
+  Console.WriteLine(numero_decimal);

+  Console.WriteLine(caracter);
+  Console.WriteLine(cadena);
+  Console.WriteLine(verdadero);
+  Console.WriteLine(falso);
   Console.ReadLine();


- static int MultiploPI(int a)
- {
-    return (int)(a * Math.PI);
- }
```

Al ejecutar la aplicación tendremos el siguiente resultado:
![Resultado tipos de datos](./content/result_types.png)

Otra cosa que deberíamos tener en cuenta es que una variable que no haya sido inicializada no se puede utilizar. Por ejemplo este caso daría error:

```diff

-  int entero1 = 10;
-  int entero2 = -10;
-  long numero_grande = 6546546950;
-  float numero_float1 = -11.5f; //Hay que poner la f porque por defecto C# trata los numeros decimales como doubles
-  float numero_float2 = float.MaxValue;
-  double numero_double = double.MaxValue;
-  decimal numero_decimal = decimal.MaxValue;

-  char caracter = '*';
-  string cadena = "Hola lemoncoders!";
-  bool verdadero = true;
-  bool falso = false;

-  Console.WriteLine(entero1);
-  Console.WriteLine(entero2);
-  Console.WriteLine(numero_grande);
-  Console.WriteLine(numero_float1);
-  Console.WriteLine(numero_float2);
-  Console.WriteLine(numero_double);
-  Console.WriteLine(numero_decimal);

-  Console.WriteLine(caracter);
-  Console.WriteLine(cadena);
-  Console.WriteLine(verdadero);
-  Console.WriteLine(falso);

+  int a;
+  Console.WriteLine(a);
   Console.ReadLine();

```

Nos queda por comentar que a partir de la versión de C# 3, las variables que se declaran en el ámbito del método pueden tener un "tipo" _var_ implícito. En este caso, es el compilador el que determina el tipo de la variable. Por este motivo, es necesario inicializar el valor en la misma línea donde se declara la variable.

Lo vemos en el siguiente ejemplo:

```diff
-   int a;
+   var entero = 5;
    Console.WriteLine(entero);
    Console.ReadLine();

```

Si ponemos el ratón encima de la variable _entero_, vemos que lo tipa de forma automática. Cabe destacar que una variable _var_ no puede tener varios tipos diferentes. Por ejemplo este código daría error.

```diff
    var entero = 5;
+   entero = "hola";
    Console.WriteLine(entero);
    Console.ReadLine();

```

A todos los efectos la variable _entero_ es de tipo _int_ a partir de su declaración.

### Referencia o Valor

Cuando trabajamos con variables tenemos que diferenciar si las estamos definiendo por valor o por referencia. Vamos a ver un caso práctico:

```csharp
    var inc = 3;
    Increment(inc);

    Console.WriteLine("Value: " + inc);

    Console.ReadLine();

    static void Increment(int value)
    {
        value = value + 1;
    }
```

Como podemos ver la variable de _inc_ no ha cambiado, porque al pasarla a la función _increment_ se pasa por valor, es decir se hace una copia y por consola el resultado de _value_ sería 3. Vamos a cambiar un poco el código:

```diff
    var inc = 3;
-   Increment(inc);
+   Increment(ref inc);

    Console.WriteLine("Value: " + inc);

    Console.ReadLine();

-   static void Increment(int value)
+   static void Increment(ref int value)
    {
        value = value + 1;
    }
```

Con la palabra reservada _ref_ delante del argumento forzamos a pasar la variable por referencia, es decir ya no se hace una copia sino que se pasa directamente la variable. El resultado cambia, ahora la variable _inc_ si ha cambiado y la consola nos devuelve que _value_ es igual a 4.

Veremos otro ejemplo en el apartado de _class_ dentro de Tipos de datos personalizados que vermos en la siguiente sección.

### Tipos de datos personalizados

Ahora que ya sabemos la diferencia que hay entre los tipos de datos por referencia o por valor. Vamos a ver los tipos de datos personalizados:

- **Enum**: Es un tipo de valor definido por un conjunto de constantes con un valor de tipo entero. Por ejemplo:

  ```csharp
  public enum DiasSemana
  {
      Lunes, //0
      Martes, //1
      Miercoles, //2
      Jueves, //3
      Viernes, //4
      Sabado, //5
      Domingo //6
  }
  ```

  De forma se le asigna los valores de forma incremental empezando desde 0 pero podemos asignarle los valores que queremos.

  ```csharp
  enum ErrorCode
  {
      None = 0,
      Unknown = 1,
      ConnectionLost = 100,
      OutlierReading = 200
  }
  ```

  Por último vamos a ver un ejemplo de cómo podemos obtener el valor o el identificador del enumerado.

  ```csharp
    Season a = Season.Autumn;
    Console.WriteLine($"Integral value of {a} is {(int)a}");  // output: Integral value of Autumn is 2
    var b = (Season)1;
    Console.WriteLine(b);  // output: Summer

    var c = (Season)4;
    Console.WriteLine(c);  // output: 4

    Console.ReadLine();

    enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }
  ```

- **Struct**: es una encapsulación de propiedades y métodos que se usan para representar una entidad.

  ```csharp
    Coords coords1 = new Coords(1, 5);
    Console.WriteLine(coords1);
    Console.ReadLine();
    struct Coords
    {
        public Coords(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X;
        public double Y;

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
  ```

  Una de las características principales de los _struct_ es qué las variables creadas son de tipo valor. Lo podemos ver en el siguiente ejemplo:

  ```diff


    Coords coords1 = new Coords(1, 5);
  + Coords coords2 = coords1;
  + coords2.X = 12;
    Console.WriteLine(coords1);
  + Console.WriteLine(coords2);
    Console.ReadLine();

    struct Coords
    {
        public Coords(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X;
        public double Y;

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
  ```

  El cambio que realizamos sobre la variable _coords2_ no afecta a _coords1_ porque las variables son copias diferentes.

- **Class**: es una encapsulación de propiedades y métodos que se usan para representar una entidad.

  ```csharp
    Persona persona1 = new Persona("Paco", "Sanchez");
    Console.WriteLine(persona1);
    Console.ReadLine();


    public class Persona
    {
        public string Nombre;
        public string Apellidos;

        public Persona(string nombre, string apellidos)
        {
            Nombre = nombre;
            Apellidos = apellidos;
        }

        public override string ToString() => $"{Apellidos}, {Nombre}";
    }
  ```

  La principal diferencia que existe entre _class_ y _struct_ es que en el caso de _class_ las variables creadas son por referencia. Lo vemos en el siguiente ejemplo:

  ```diff

    Persona persona1 = new Persona("Paco", "Sanchez");
  + Persona persona2 = persona1;
  + persona2.Nombre = "Fran";

    Console.WriteLine(persona1);
  + Console.WriteLine(persona2);

    Console.ReadLine();


    public class Persona
    {
        public string Nombre;
        public string Apellidos;

        public Persona(string nombre, string apellidos)
        {
            Nombre = nombre;
            Apellidos = apellidos;
        }

        public override string ToString() => $"{Apellidos}, {Nombre}";
    }
  ```

  El nombre cambia en las dos variables cuando modifico cualquiera de ellas porque ambas apuntan a un mismo espacio de memoria.

  Existen otras diferencias cómo: Los _struct_ no aceptan _null_, con las _classes_ podemos usar herencia, polimorfismo, etc...

  **Profundizaremos en todas la posibilidades que nos da las Clases en el siguiente apartado.**

- **Record**: En la version 9 de C# se introduce esta palabra reservada. Un objeto declarado como record funciona de forma muy similar a como lo hace _struct_, pero lo hará por referencia. Es decir está entre los _struct_ y las _clases_.

  Vamos a empezar a analizarlo. Creamos un struct:

  ```csharp
    var person = new Person("John");
    var otherPerson = person;
    otherPerson.Name = "Peter";

    Console.WriteLine(person);
    Console.WriteLine(otherPerson);

    Console.ReadLine();


    public struct Person
    {
        public string Name;

        public Person(string name)
        {
            Name = name;
        }

        public override string ToString() => $"Mi nombre es {Name}";
    }
  ```

  Como ya hemos visto se copia la variable y cuando se modifica una no implica la modificación de la otra. Vamos a ver qué pasa si comprobamos si son iguales estas variables.

```diff
    var person = new Person("John");
    var otherPerson = person;
    otherPerson.Name = "Peter";

    Console.WriteLine(person);
    Console.WriteLine(otherPerson);
+   Console.WriteLine("Equals: " + person.Equals(otherPerson));
+   Console.WriteLine("Reference: " + (person == otherPerson));

    Console.ReadLine();


    public struct Person
    {
        public string Name;

        public Person(string name)
        {
            Name = name;
        }

        public override string ToString() => $"Mi nombre es {Name}";
    }
```

Lo primero que observamos es que con _struct_ no podemos usar el operador de igual porque no esta permitido (si fuera clases si nos dejaría). Vamos a comentar esa línea y vemos el resultado.

```diff

  var person = new Person("John");
  var otherPerson = person;
  otherPerson.Name = "Peter";

  Console.WriteLine(person);
  Console.WriteLine(otherPerson);
  Console.WriteLine("Equals: " + person.Equals(otherPerson));
-   Console.WriteLine("Reference: " + (person == otherPerson));
+   // Console.WriteLine("Reference: " + (person == otherPerson));
  Console.ReadLine();


  public struct Person
  {
      public string Name;

      public Person(string name)
      {
          Name = name;
      }

      public override string ToString() => $"Mi nombre es {Name}";
  }
```

Como hemos modificado el nombre el resultado es _false_ pero ¿qué pasa si no modificamos el nombre?

```diff

    var person = new Person("John");
    var otherPerson = person;
-    otherPerson.Name = "Peter";
+    // otherPerson.Name = "Peter";

    Console.WriteLine(person);
    Console.WriteLine(otherPerson);
    Console.WriteLine("Equals: " + person.Equals(otherPerson));
    // Console.WriteLine("Reference: " + (person == otherPerson));
    Console.ReadLine();


    public struct Person
    {
        public string Name;

        public Person(string name)
        {
            Name = name;
        }

        public override string ToString() => $"Mi nombre es {Name}";
    }

```

Perfecto, el resultado es true porque ambas variables tienen el mismo contenido. Bueno vamos a ver qué pasa si hacemos lo mismo con clases (vamos a des comentar el código):

```diff
    var person = new Person("John");
    var otherPerson = person;
-    // otherPerson.Name = "Peter";
+    otherPerson.Name = "Peter";

    Console.WriteLine(person);
    Console.WriteLine(otherPerson);
    Console.WriteLine("Equals: " + person.Equals(otherPerson));
-   // Console.WriteLine("Reference: " + (person == otherPerson));
+   Console.WriteLine("Reference: " + (person == otherPerson));
    Console.ReadLine();


-   public struct Person
+   public class Person
    {
        public string Name;

        public Person(string name)
        {
            Name = name;
        }

        public override string ToString() => $"Mi nombre es {Name}";
    }
```

En el caso de clases siempre nos dará _true_ cuando comparemos las variables, porque ambas están apuntando a la misma referencia. Y ahora llega el momento de comprobar que pasará con record

> Ojo: Este ejemplo solo es válido si hemos seleccionado a partir de la versión .Net 5.0 a la hora de crear la solución.

```diff
    var person = new Person("John");
    var otherPerson = person;
    otherPerson.Name = "Peter";

    Console.WriteLine(person);
    Console.WriteLine(otherPerson);
    Console.WriteLine("Equals: " + person.Equals(otherPerson));
    Console.WriteLine("Reference: " + (person == otherPerson));
    Console.ReadLine();


-   public class Person
+   public record Person
    {
        public string Name;

        public Person(string name)
        {
            Name = name;
        }

        public override string ToString() => $"Mi nombre es {Name}";
    }
```

El resultado es el mismo con las clases porque es una variable por referencia, es decir al hacer la asignación estamos haciendo que ambas variables tenga la misma referencia y por tanto si modificamos una se modifica las dos.

¿Pero qué pasará en el siguiente ejemplo?

```diff
    var person = new Person("John");
-   var otherPerson = person;
-    otherPerson.Name = "Peter";
+   var otherPerson = new Person("John");

    Console.WriteLine(person);
    Console.WriteLine(otherPerson);
    Console.WriteLine("Equals: " + person.Equals(otherPerson));
    Console.WriteLine("Reference: " + (person == otherPerson));
    Console.ReadLine();


    public record Person
    {
        public string Name;

        public Person(string name)
        {
            Name = name;
        }

        public override string ToString() => $"Mi nombre es {Name}";
    }
```

Ahora nos está comparando el contenido de la variable, y como ambas tienen el mismo _name_ el resultado en ambos casos es _true_. Sin embargo con _class_ este resultado sería _false_ porque son instancias diferentes.

## Operadores

C# proporciona una serie de operadores compatibles con los tipos integrados que permiten realizar operaciones básicas. Se pueden clasificar en:

- Aritméticos: operaciones aritméticas con operandos numéricos.
- Comparación: comparan operandos numéricos.
- Lógicos: realizan operaciones lógicas con operandos bool.
- Igualdad: comprueban si sus operandos son iguales o no.

Vamos a ver algunos ejemplos:

```c#
    var a = 3 + 4;
    var b = 3 - 4;
    var c = 3 * 4;
    var d = 3 / 4;
    var e = 3 % 4;

    Console.WriteLine(a);
    Console.WriteLine(b);
    Console.WriteLine(c);
    Console.WriteLine(d);
    Console.WriteLine(e);

    Console.ReadLine();
```

También existe dentro los operadores aritméticos los operadores de incremento o decremento (++, --) que se pueden usar tanto a la derecha como a la izquierda. Vemos la diferencia con un ejemplo:

```c#
    var a = 1;
    var b = 1;

    Console.WriteLine(++a);
    Console.WriteLine(b++);

    Console.ReadLine();
```

Cuando usamos el operador de incremento a la izquierda el valor se actualiza en esa misma línea, por tanto en el ejemplo se muestra el valor _2_, pero si lo usamos a la derecha, el resultado no se actualiza hasta las siguiente consulta a la variable, por tanto en el ejemplo sigue teniendo el valor _1_.

Otra forma que podemos utilizar para hacer incremento o decremento de N unidades es usando el operador += + -=. Lo vemos con el ejemplo:

```c#

    var a = 1;
    var b = 1;

    a += 10;
    b -= 10;

    Console.WriteLine(a);
    Console.WriteLine(b);

    Console.ReadLine();
```

Por último, podemos utilizar el operador aritmético _+_ para concatenar cadenas de texto, pero se suele utilizar la interpolación con el operador $. Lo vemos con el siguiente ejemplo:

```c#

    var a = "Hola";
    var b = "Lemoncoders";

    var cad = $"{a}, {b}";

    Console.WriteLine(cad);

    Console.ReadLine();
```

## Casting / Conversion de tipos

Como hemos dicho en el principio del curso, C# es un lenguaje altamente tipado por tanto no nos permite "directamente" hacer la siguiente asignación:

```c#
int i;
i = "Hello";
```

Pero es posible que en ocasiones necesitamos copiar un valor en una variable o parámetro de otro tipo. Por ejemplo, es posible que tenga una variable de _entero_ que se necesita pasar a un método cuyo parámetro es de tipo _double_.

En C#, se pueden realizar las siguientes conversiones de tipos:

- **Conversión explicita (Casting)**: se produce entre tipos incompatibles por lo que tenemos que especificar a qué tipo se va a convertir o crearnos nuestro propio método.
- **Conversión implícita (Sin notación)**: se produce entre tipos compatibles pero con diferentes alcance.

Un ejemplo de conversión implícita puede ser:

```c#
int num = 2147483647;
long bigNum = num;
```

Ojo que si lo hacemos al revés (de long a int) necesitaríamos una conversión explicita y si el número no se puede representar con int nos daría un valor totalmente diferente al esperado.

```c#
long bigNum = 2147483647;
int num = (int)bigNum;
```

Por otro lado, para hacer conversiones de tipos más "potentes" tenemos a nuestra disposición Clases o Métodos para, por ejemplo, comprobar si es posible hacer una conversión sin lanzarnos una excepción.

```c#
string x = "10";
int x1 = (int) x; // Da un error en tiempo de diseño
int x2 = Convert.ToInt(x);

string y = "10a";
int y2;
bool ok = int.TryParse(y, out y2);
```

## Bucles y Condicionales

Como en todo lenguaje de programación tenemos condicionales y bucles.

## Condicionales

- **if**

Con ls estructura condicional **if** podemos ejecutar un bloque de código u otro dependiendo de una condición.

Para ello podemos utilizar:

- Operadores de comparación ( ==, !=, <, <=, >, >=)
- Operadores lógicos (|| , &&)
- Operador de negación (!)

```csharp
    Console.WriteLine("Introduce tu edad:");
    var edadUsuario = Console.ReadLine();
    var edad = Convert.ToInt32(edadUsuario);

    if (edad > 18)
    {
        Console.WriteLine("Eres mayor de edad");
    }

    Console.ReadLine();

```

Si sólo hay una linea de código en el bloque de la condición se puede eliminar las llaves (pero no es recomendable)

- **if-else**

Si queremos ejecutar un bloque de código u otro según una condición, podemos utilizar la estructura condicional _if...else_:

```csharp
    Console.WriteLine("Introduce tu edad:");
    var edadUsuario = Console.ReadLine();
    var edad = Convert.ToInt32(edadUsuario);

    if(edad > 18)
    {
        Console.WriteLine("Eres mayor de edad");
    }
    else
    {
        Console.WriteLine("Eres menor de edad");
    }

    Console.ReadLine();
```

- **if-elseif**

Cuando necesitamos introducir otra estructura condicional dentro de algún bloque _if_, estamos anidando sentencias _if_. Esto se considera una mala práctica, ya que impacta en la legibilidad de nuestro código.

```csharp

    Console.WriteLine("Introduce tu edad:");
    var edadUsuario = Console.ReadLine();
    var edad = Convert.ToInt32(edadUsuario);

    if(edad >= 18)
    {
        Console.WriteLine("Eres adulto");
    }
    else
    {
        if (12 < edad && edad < 18) {
            Console.WriteLine("Eres adolescente");
        }
        else {
            Console.WriteLine("Eres un niño");
        }
    }

```

Para ello se puede usar la estructura condicional _if...elseif_:

```diff

    Console.WriteLine("Introduce tu edad:");
    var edadUsuario = Console.ReadLine();
    var edad = Convert.ToInt32(edadUsuario);

    if(edad >= 18)
    {
        Console.WriteLine("Eres adulto");
    }
-   else
-   {
-       if (12 < edad && edad < 18) {
-           Console.WriteLine("Eres adolescente");
-       }
-       else {
-           Console.WriteLine("Eres un niño");
-       }
-   }
+   else if (12 < edad && edad < 18)
+   {
+       Console.WriteLine("Eres adolescente");
+   }
+   else
+   {
+       Console.WriteLine("Eres un niño");
+   }

    Console.ReadLine();
```

- **switch**

Otra estructura condicional que podemos utilizar para ejecutar diferentes bloques de código es el _Switch_. En este caso _switch_ necesita una expresión de control que utilizará para evaluar, en el caso de coincidir con algún caso definido ejecutará el bloque asociado. Por ejemplo:

```csharp
    Console.WriteLine("Introduce día de la semana:");
    var diaSemana = Console.ReadLine().ToLower();

    switch (diaSemana)
    {
        case "lunes":
        case "martes":
        case "miercoles":
        case "jueves":
        case "viernes":
            Console.WriteLine("Es día laboral, a trabajar!");
            break;
        case "sabado":
        case "domingo":
            Console.WriteLine("Es fin de semana, a descansar que te lo has ganado");
            break;
        default:
            Console.WriteLine("Introduce un día de la semana");
            break;
    }

    Console.ReadLine();
```

Es importante destacar que necesitamos utilizar la palabra reservada _break_ para forzar la salida del _switch_ en otro caso seguiría evaluando las siguientes expresiones.

También disponemos de un bloque _default_ que ejecutará si no se ha cumplido ninguno de los casos anteriores.

Por último comentar que cada expresión de los casos debe ser única y sólo puede contener expresiones constantes (int, char, String)

- **Operador ternario**

El operador condicional (?), también conocido como operador condicional ternario, evalúa una expresión booleana y devuelve el resultado de una de las dos expresiones, en función de que la expresión booleana se evalúe como true o false, tal y como se muestra en el siguiente ejemplo:

```csharp
    Console.WriteLine("Introduce tu edad:");
    var edad = Convert.ToInt32(Console.ReadLine());

    var resultado = edad >= 18 ? "Eres mayor de edad" : "Eres menor de edad";

    Console.WriteLine(resultado);
    Console.ReadLine();
```

- **Operador de valor nulo (Null coalescing operator)**

Con este operador podemos asegurar que un valor es diferente de nulo, y en el caso de que sea nulo le damos un valor por defecto. Es decir, en una sola línea el operador recibe dos operandos y devuelve el primero que no sea nulo.

```csharp
    string nombre = null;
    string persona = nombre ?? "persona sin identificar";
    Console.WriteLine(persona);

    nombre = "Pepe";

    persona = nombre ?? "persona sin identificar";

    Console.WriteLine(persona);

    Console.ReadLine();
```

## Bucles

Con los bucles conseguimos alterar el flujo de un programa para permitirnos repetir un bloque hasta que una condición se cumpla.

- **while**

Con este tipo de bucle el código se ejecutará hasta que la condición sea falsa. La variable que esté en la condición ha de variar para no provocar un bucle infinito.

```csharp

    int numero = 0;

    while (numero <= 5)
    {
        Console.WriteLine(numero);
        numero += 1;
    }

    Console.ReadLine();
```

- **do-while**

Es parecida al while, pero en este caso se va a ejecutar al menos 1 vez, mientras que el while se puede ejecutar 0 o N veces.

```csharp
    int numero;

    do
    {
        Console.WriteLine("Introduce un número entre 0 y 5:");
        numero = Convert.ToInt32(Console.ReadLine());
    } while (numero >= 5 || numero < 0);

    Console.WriteLine($"El número introducido es: {numero}");
    Console.ReadLine();
```

- **for**

En este caso, además de la condición se incluye la inicialización de la variable, así como el incremento o decremento de la misma.

```csharp

    for (var numero = 1; numero <= 5; numero++)
    {
        Console.WriteLine(numero);
    }
    Console.ReadLine();
```

- **foreach**

Este bucle se utiliza para recorrer colecciones de datos. (Lo veremos más adelante)

```csharp

    List<string> nombres = new List<string>
    {
        "Ana",
        "Pedro",
        "Pepe",
        "Maria"
    };

    foreach(var nombre in nombres)
    {
        Console.WriteLine($"El nombre es: {nombre}");
    }
    Console.ReadLine();

```
