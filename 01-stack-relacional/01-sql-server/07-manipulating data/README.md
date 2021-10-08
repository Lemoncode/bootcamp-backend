# SQL SERVER - DML

Vamos a aprender y practicar cómo manipular datos de nuestra base de datos. De ahí las siglas DML (Data Manipulation Language). Recordemos que trabajamos con Transact SQL (T-SQL), el lenguaje de Microsoft SQL Server.

Para los ejemplos utilizaremos las bases de datos de **AdventureWorksLT**, una versión ligera de la base de datos que hemos restaurado en el paso anterior (_06-restore-sample-database)_.

La tenéis disponible en [este enlace](https://docs.microsoft.com/es-es/sql/samples/adventureworks-install-configure?view=sql-server-ver15&tabs=ssms).

## QUERY

Una `Query` es una **consulta** a la base de datos para recuperar información de las tablas.

Utilizaremos la cláusula `SELECT`.

## SELECT

Podemos pensar que una `Query` es una pregunta que le hacemos a la base de datos para recuperar los datos que necesitamos:

- ¿Cuál es el nombre y el teléfono de todos mis clientes?
- ¿Cuáles son los pedidos de hoy?
- ¿Cuáles son los emails de los clientes que han realizado un pedido hoy?

Todas estas preguntas tienen algo en común: la entidad (tabla) que quiero obtener (por ejemplo, clientes) y la información que quiero de cada uno (columnas, por ejemplo nombre y teléfono).

Una sentencia `SELECT` necesita ambas para formarse:

```sql
SELECT <columnas a preguntar> FROM <tabla>;
```

Así, podríamos crear la siguiente `Query` para realizar la primera pregunta: _¿Cuál es el nombre y el teléfono de todos mis clientes?_:

```sql
SELECT Nombre, Telefono FROM Cliente;
```

Aunque no es buena práctica para código final de nuestra aplicación, puede ser que necesitemos preguntar **todas** las columnas de la tabla. Para esto podemos utilizar el carácter `*`:

```sql
SELECT * FROM Cliente; -- BAD PRACTICE
```

> Recomendamos no utilizarlo para código de la aplicación, sólo si estamos desarrollando y necesitamos jugar en _SQL Server Management Studio_ o _Azure Data Studio_ con las tablas de desarrollo para obtener información.

Es buena práctica, nombrar la tabla antes de las columnas que queremos recuperar. Aunque estos primeros ejemplos son muy sencillos, será muy útil esta práctica cuando tengamos consultas más complicadas implicando varias tablas:

```sql
SELECT Cliente.Nombre, Cliente.Telefono FROM Cliente;
-- GOOD PRACTICE
```

¿Supone escribir demasiado esta buena práctica? Es cierto, pero podemos utilizar `Alias` para simplificar nuestro código:

```sql
SELECT C.Nombre, C.Telefono FROM Cliente C;
```

> En esta consulta, hemos asignado el alias `C` a la tabla `Cliente`. Por tanto podemos referirnos a ella y obtener sus columnas como `C.<columna>`.

Algunos ejemplos sobre la base de datos LT:

```sql
-- ¿Cuales son los artículos de mi tienda?:
SELECT  P.ProductID,
        P.Name,
        P.Color,
        P.Size,
        P.ListPrice
FROM    SalesLT.Product P


-- ¿Cual es el nombre y teléfono de mis clientes?:
SELECT	C.CustomerID,
        C.FirstName,
        C.LastName,
        C.Phone
FROM	SalesLT.Customer C
```

## WHERE

Muy bien, ya sabemos recuperar **todos** los registros de una tabla, como en los ejemplos anteriores... Pero, ¿y si queremos filtrar esos resultados? Por ejemplo, listar los artículos de mi tienda de color `White`, o listar mis clientes que se llaman `Luis`.

Para filtrar los resultados utilizaremos la cláusula `WHERE` y la condición (o condiciones) por la que queremos filtrar.

- Se coloca después de la cláusula `FROM`.
- Utiliza expresiones booleanas.
- Sólo las filas coincidentes se devolverán en el conjunto de resultados.

```sql
SELECT  <columnas>
FROM    <tabla>
WHERE   <condición>
```

Ejemplos:

```sql
-- Listar los artículos de mi tienda de color `White`:
SELECT  P.ProductID,
        P.Name,
        P.Color,
        P.Size,
        P.ListPrice
FROM    SalesLT.Product P
WHERE   P.Color = 'White'

-- Listar mis clientes que se llaman `Luis`:
SELECT	C.CustomerID,
        C.FirstName,
        C.LastName,
        C.Phone
FROM	SalesLT.Customer C
WHERE   C.FirstName = 'Luis'
```

### Combinar condiciones: Operadores AND / OR

Puede que necesitemos combinar más de una condición para filtrar los resultados. Podemos utilizar los operadores `AND` y `OR`:

```sql
-- Listar los artículos de mi tienda de color `White` y talla `L`:
SELECT  P.ProductID,
        P.Name,
        P.Color,
        P.Size,
        P.ListPrice
FROM    SalesLT.Product P
WHERE   P.Color = 'White'
        AND P.Size = 'L'

-- Listar mis clientes que se llaman `Luis` o `David`:
SELECT	C.CustomerID,
        C.FirstName,
        C.LastName,
        C.Phone
FROM	SalesLT.Customer C
WHERE   C.FirstName = 'Luis'
        OR C.FirstName = 'David'
```

### Otros operadores

Podemos utilizar otros operadores para filtrar nuestros resultados, por ejemplo:

- Mis clientes cuyo nombre comienza por `L`.
- Mis pedidos realizados entre dos fechas dadas.
- Mis artículos que no tienen talla (columna `Size` es `NULL`).

#### LIKE

Hemos podido obtener, con `WHERE FirstName = 'Luis'` aquellos clientes que se llaman `Luis`. Pero, ¿y si quiero filtrar sólo por una parte coincidente del campo de texto? Por ejemplo, aquellos clientes que comienzan por `L`.

Podemos utilizar el operador `LIKE` y el caracter `%` para especificar "cualquier texto":

```sql
-- Listar mis clientes que comienzan por L:
SELECT	C.CustomerID,
        C.FirstName,
        C.LastName,
        C.Phone
FROM	SalesLT.Customer C
WHERE   C.FirstName LIKE 'L%'
```

Podemos situar el caracter `%` donde lo necesitemos, dependiendo de la pregunta que queremos hacer. Otro ejemplo: clientes cuyo nombre acaba en L:

```sql
-- Listar mis clientes que acaban en L:
SELECT	C.CustomerID,
        C.FirstName,
        C.LastName,
        C.Phone
FROM	SalesLT.Customer C
WHERE   C.FirstName LIKE '%L'
```

Así nos listará todos los `Daniel`, `Michael`, `Gabriel`, etc...

¿Y si queremos listar todos aquellos que contengan una L?:

```sql
-- Listar mis clientes que contienen al menos una L:
SELECT	C.CustomerID,
        C.FirstName,
        C.LastName,
        C.Phone
FROM	SalesLT.Customer C
WHERE   C.FirstName LIKE '%L%'
```

#### BETWEEN

El operador `BETWEEN` nos ayudará a filtrar nuestros resultados que coincidan con un **rango** o intervalo de valores:

- Clientes que han modificado sus datos en Mayo de 2009.
- Pedidos que se realizaron en el año 2008.
- Artículos de mi tienda que cuestan entre 50€ y 100€.
-

```sql
-- Clientes que han modificado sus datos en Mayo de 2009:
SELECT	C.CustomerID,
        C.FirstName,
        C.LastName,
        C.Phone
FROM	SalesLT.Customer C
WHERE   C.ModifiedDate BETWEEN '2009-05-01' AND '2009-05-31'

-- Pedidos que se realizaron en el año 2008:
SELECT	O.SalesOrderID,
		O.CustomerID,
		O.SubTotal,
		O.TotalDue
FROM	SalesLT.SalesOrderHeader O
WHERE	O.OrderDate BETWEEN '2008-01-01' AND '2008-12-31'

-- Artículos de mi tienda que cuestan entre 50€ y 100€:
SELECT  P.ProductID,
        P.Name,
        P.Color,
        P.Size,
        P.ListPrice
FROM    SalesLT.Product P
WHERE   P.ListPrice BETWEEN 50 AND 100
```

#### IN

El operador `IN` es una alternativa valiosa cuando queremos filtrar un campo por varios valores (en lugar de hacer varios `OR` sobre el mismo campo). Por ejemplo:

- Artículos de mi tienda en color blanco, negro o amarillo.

Podríamos crear la siguiente consulta con el operador `OR`:

```sql
-- Artículos de mi tienda en color blanco, negro o amarillo:
SELECT  P.ProductID,
        P.Name,
        P.Color,
        P.Size,
        P.ListPrice
FROM    SalesLT.Product P
WHERE	P.Color = 'White'
		OR P.Color = 'Black'
		OR P.Color = 'Yellow'
```

Cuantos más valores de la columna `Color` queramos filtrar, más operadores `OR` debemos tener. Utilizar el operador `IN` nos puede simplificar nuestras consultas en estos casos:

```sql
-- Artículos de mi tienda en color blanco, negro o amarillo:
SELECT  P.ProductID,
        P.Name,
        P.Color,
        P.Size,
        P.ListPrice
FROM    SalesLT.Product P
WHERE	P.Color IN ('White', 'Black', 'Yellow')
```

#### IS

El operador `IS` nos ayudará a comparar campos con el valor `NULL`. Si quisieramos filtrar una columna por este valor, no podríamos hacer `WHERE <columna> = NULL`:

```sql
-- Artículos de mi tienda que no tienen talla:
SELECT  P.ProductID,
        P.Name,
        P.Color,
        P.Size,
        P.ListPrice
FROM    SalesLT.Product P
WHERE	P.Size = NULL

-- Esto devolverá 0 resultados
```

> La consulta anterior devolverá 0 resultados. Para comparar con valores `NULL` debemos utilizar el operador `IS`

Para esto debemos utilizar el operador `IS`:

```sql
-- Artículos de mi tienda que no tienen talla:
SELECT  P.ProductID,
        P.Name,
        P.Color,
        P.Size,
        P.ListPrice
FROM    SalesLT.Product P
WHERE	P.Size IS NULL

-- 84 resultados
```

#### NOT

¿Qué ocurre si queremos hacer la negación de todas las consultas anteriores?

- Listar mis clientes que **NO** comienzan por L.
- Artículos de mi tienda que **NO** cuestan entre 50€ y 100€.
- Artículos de mi tienda en colores que **NO** sean blanco, negro o amarillo.
- Artículos de mi tienda que tienen talla (`Size` es **NO** `NULL`).

Para esto utilizaremos el operador `NOT` sobre los operadores anteriores:

```sql
-- Listar mis clientes que NO comienzan por L:
SELECT	C.CustomerID,
        C.FirstName,
        C.LastName,
        C.Phone
FROM	SalesLT.Customer C
WHERE   C.FirstName NOT LIKE 'L%'

-- Artículos de mi tienda que NO cuestan entre 50€ y 100€:
SELECT  P.ProductID,
        P.Name,
        P.Color,
        P.Size,
        P.ListPrice
FROM    SalesLT.Product P
WHERE   P.ListPrice NOT BETWEEN 50 AND 100

-- Artículos de mi tienda en colores que NO sean blanco, negro o amarillo:
SELECT  P.ProductID,
        P.Name,
        P.Color,
        P.Size,
        P.ListPrice
FROM    SalesLT.Product P
WHERE	P.Color NOT IN ('White', 'Black', 'Yellow')

-- Artículos de mi tienda que tienen talla:
SELECT  P.ProductID,
        P.Name,
        P.Color,
        P.Size,
        P.ListPrice
FROM    SalesLT.Product P
WHERE	P.Size IS NOT NULL
```

## Funciones

### COUNT

La función `COUNT` nos devolverá el recuento de la columna especificada (incluye valores NULL si especificamos `*`):

```sql
-- ¿Cuantos clientes tengo que se llaman Alice?
SELECT  COUNT(*)
FROM    SalesLT.Customer C
WHERE   C.FirstName = 'Alice';
-- Reslult: 4

-- ¿Cuantos clientes tengo que se llaman Alice y que tienen MiddleName?
SELECT  COUNT(C.MiddleName)
FROM    SalesLT.Customer C
WHERE   C.FirstName = 'Alice';
-- Reslult: 2
```

> Si especificamos una columna (en lugar de `*`) quitará del recuento aquellas filas en las que el valor de la columna sea `NULL`

### MAX

Devuelve el valor máximo de la columna especificada:

```sql
-- ¿Cual es el precio máximo de todos mis pedidos?
SELECT  MAX(TotalDue)
FROM    SalesLT.SalesOrderHeader;
```

### MIN

Devuelve el valor mínimo de la columna especificada:

```sql
-- ¿Cual es el precio mínimo de todos mis pedidos?
SELECT  MIN(TotalDue)
FROM    SalesLT.SalesOrderHeader;
```

### AVG

Devuelve la media de todos los valores de la columna especificada (no incluye NULL, sólo columnas numéricas)

```sql
-- ¿Cual es el precio medio de todos mis pedidos?
SELECT  AVG(TotalDue)
FROM    SalesLT.SalesOrderHeader;
```

### SUM

```sql
-- ¿Cual es el precio total sumando todos mis pedidos?
SELECT  SUM(TotalDue)
FROM    SalesLT.SalesOrderHeader;
```

## Filas de resultado únicas

Podemos quitar de los resultados filas repetidas (filas cuyas columnas tienen los mismos valores) con la cláusula `DISTINCT`:

```sql
-- Listar todos los nombres (sin repetir) de mis clientes:
SELECT  DISTINCT(C.FirstName)
FROM    SalesLT.Customer C;
```

> Con la consulta anterior, si tengo 10 clientes que se llaman `Luis` sólo aparecerá una vez en los resultados.

## Agrupando filas

Podemos agrupa los resultados en Subsets con la cláusula `GROUP BY <columnas>`. Los subsets estarán dictados por las columnas definidas en el `GROUP BY`:

```sql
SELECT  C.FirstName
FROM    SalesLT.Customer C
GROUP BY C.FirstName;
```

Parece que el resultado obtenido es algo parecido a lo que conseguimos con `DISTINCT`, pero ahora podemos aplicar funciones a los subsets que se generan. Por ejemplo:

- Listar el nombre de los clientes con el número de veces que se repiten.

```sql
-- Listar el nombre de los clientes con el número de veces que se repiten.
SELECT  C.FirstName, COUNT(*) as Recuento
FROM    SalesLT.Customer C
GROUP BY C.FirstName
```

Podemos ver cómo hemos añadido un alias a la columna que se genera con el recuento que devuelve `COUNT`.

Ahora tendríamos el siguiente resultado:

```
FirstName	Recuento
---------------------
Abigail	        2
Abraham	        2
Aidan	        2
Ajay	        2
Alan	        4
Alberto	        1
Alexander  	    4
Alice	        4
Alvaro	        2
Amy	            2
...
```

#### Filtrando filas en las agrupaciones

Del resultado de la consulta anterior, podríamos querer filtrar por aquellos cuya repetición (columna `Recuento`) es mayor de 10, para ver qué nombre de cliente se repite más. Podríamos pensar en hacer algo como:

```sql
SELECT  C.FirstName, COUNT(*) as Recuento
FROM    SalesLT.Customer C
GROUP BY C.FirstName
WHERE   Recuento > 10

-- Incorrect syntax near the keyword 'WHERE'
```

Vemos que nos devuelve un error, debido a que la cláusula `WHERE` aplica sobre `SELECT ... FROM`, **antes de aplicar las agrupaciones.**

Para filtrar las agrupaciones utilizaremos la cláusula `HAVING`:

```sql
-- Listar el nombre de los clientes que más veces se repiten (> 10)
SELECT  C.FirstName, COUNT(*) as Recuento
FROM    SalesLT.Customer C
GROUP BY C.FirstName
HAVING  COUNT(*) > 10
```

Ahora sí tenemos el resultado esperado:

```
FirstName	Recuento
--------------------
David	        11
John	        20
Michael	        15
Robert	        22
Scott	        12
```

## Cruzando tablas (JOIN)

Vamos a necesitar cruzar dos o más tablas para recuperar campos relacionados. Esto es posible gracias a la cláusula `JOIN`. Vamos a ver cada tipo:

### CROSS JOIN

- Es el JOIN más simple
- Cruza todas las filas de ambas tablas
- ¡Ineficiente! (No queremos usar este tipo de Query!)
- Producto Cartesiano

```sql
SELECT  T.NombreTipo, T2.NombreTipo
FROM    dbo.TipoArticulo T
CROSS JOIN dbo.TipoCliente T2
```

> No iguala ninguna columna, por tanto devolverá el cruce de todas las filas de la primera tabla con todas las filas de la segunda tabla. Si tenemos 4 filas en cada una, obtendremos 16 filas como resultado. ¡Imagina con 2000 filas en cada tabla! Rara vez se justificará el uso de este `JOIN`.

### INNER JOIN

Podemos cruzar dos tablas y relacionarlas por alguna/s de sus columnas. Por ejemplo, tenemos la tabla `SalesOrderHeader` con los pedidos y el ID del cliente (`CustomerId`), pero no tenemos la información de qué cliente es. Tendremos que cruzar esta tabla de pedidos, con la tabla de clientes (`Customer`), relacionando sus campos `CustomerId`:

```sql
-- Listar los pedidos con la información del cliente que lo ha realizado:
SELECT	O.SalesOrderID,
		C.FirstName,
		C.LastName,
		C.EmailAddress
FROM	SalesLT.SalesOrderHeader O
INNER JOIN SalesLT.Customer C
	ON C.CustomerID = O.CustomerID
```

Podemos añadir más tablas al juego, por ejemplo, la tabla `Address` que contiene la dirección de los clientes, y la necesitamos para enviar el pedido:

```sql
-- Listar los pedidos con el cliente que lo ha realizado y su dirección:
SELECT	O.SalesOrderID,
		C.FirstName,
		C.LastName,
		C.EmailAddress,
		A.AddressLine1,
		A.PostalCode,
		A.City,
		A.StateProvince
FROM	SalesLT.SalesOrderHeader O
INNER JOIN SalesLT.Customer C
	ON C.CustomerID = O.CustomerID
INNER JOIN SalesLT.CustomerAddress CA
	ON CA.CustomerID = C.CustomerID
INNER JOIN SalesLT.Address A
	ON A.AddressID = CA.AddressID
```

## OUTER JOIN

Los `OUTER JOIN` nos va a permitir contemplar los valores `NULL` de la relación de columnas en nuestros resultados.

Es util cuando no queremos perder resultados de nuestra tabla principal, si no existe un registro relacionado en la tabla secundaria.

Vamos a entenderlo, paso a paso:

- La siguiente consulta, devuelve todos los clientes que tenemos registrados (847 filas):

```sql
SELECT	C.CustomerID,
		C.FirstName
FROM	SalesLT.Customer C
```

- Si cruzamos con `INNER JOIN` con la tabla de pedidos, para tener clientes y pedidos, nos devolverá 32 filas:

```sql
SELECT	C.CustomerID,
		C.FirstName,
		O.TotalDue
FROM	SalesLT.Customer C
INNER JOIN SalesLT.SalesOrderHeader O
	on O.CustomerID = C.CustomerID
```

Esto nos indica que hay 32 pedidos y por tanto muchos clientes no han realizado ningún pedido (al hacer el cruce con `INNER JOIN` hemos perdido los clientes que no están en la tabla de pedidos).

¿Y si queremos un listado de todos los clientes y sus pedidos incluyendo aquellos clientes que no han realizado ninguno?

Para esto utilizaremos `OUTER JOIN`, y tenemos 3 tipos dependiendo en que parte está nuestra tabla principal, si está a la izquierda (`LEFT JOIN`), derecha (`RIGHT JOIN`) o aceptamos `NULL` en ambas (`FULL JOIN`).

### LEFT JOIN

Para esto podemos utilizar `LEFT JOIN` y permitir que en la tabla secundaria (pedidos) no encuentre al cliente (y por tanto devuelva todos los campos de esta tabla a `NULL` para ese registro):

```sql
SELECT	C.CustomerID,
		C.FirstName,
		O.TotalDue
FROM	SalesLT.Customer C
LEFT JOIN SalesLT.SalesOrderHeader O
	on O.CustomerID = C.CustomerID
```

En los resultados veremos cómo nos devuelve muchas filas con los campos de la tabla de pedidos a `NULL`, pero no perdemos esos registros:

```
CustomerID	FirstName	TotalDue
--------------------------------
...
29530	    Daniel	        NULL
29531	    Cory	    7330,8972
29532	    James	        NULL
29533	    Douglas	        NULL
29535	    Wayne	        NULL
29536	    Robert	        NULL
29539	    Josh	        NULL
29541	    Gytis	        NULL
29544	    Shaun	        NULL
29545	    John	        NULL
29546	    Christopher	98138,2131
29548	    Ann	            NULL
29550	    Stanley	        NULL
29553	    Edna	        NULL
...
```

Además con `LEFT JOIN` podemos aprovechar para filtrar por aquellos clientes que aún no han realizado ningún pedido (si queremos enviarle alguna oferta):

```sql
SELECT	C.CustomerID,
		C.FirstName,
		O.TotalDue
FROM	SalesLT.Customer C
LEFT JOIN SalesLT.SalesOrderHeader O
	on O.CustomerID = C.CustomerID
WHERE   O.CustomerID IS NULL
```

### RIGHT JOIN

Actúa igual que `LEFT JOIN` pero entendiendo que la tabla principal está a la derecha:

```sql
SELECT	C.CustomerID,
		C.FirstName,
		O.TotalDue
FROM	SalesLT.SalesOrderHeader O
RIGHT JOIN SalesLT.Customer C
	on C.CustomerID = O.CustomerID
WHERE   O.CustomerID IS NULL
```

## VIEWS

Por ejemplo, la consulta _Listar los pedidos con el cliente que lo ha realizado y su dirección_ seguramente la necesitemos en varios puntos de nuestra aplicación.

Podemos crear una nueva vista (`VIEW`) con ella, y así poder reutilizarla e incluso hacer queries sobre sus resultados:

```sql
CREATE VIEW SalesLT.vPedidosClientesDireccion
AS
SELECT	O.SalesOrderID,
		C.FirstName,
		C.LastName,
		C.EmailAddress,
		A.AddressLine1,
		A.PostalCode,
		A.City,
		A.StateProvince
FROM	SalesLT.SalesOrderHeader O
INNER JOIN SalesLT.Customer C
	ON C.CustomerID = O.CustomerID
INNER JOIN SalesLT.CustomerAddress CA
	ON CA.CustomerID = C.CustomerID
INNER JOIN SalesLT.Address A
	ON A.AddressID = CA.AddressID;

GO
```

Ahora podemos incluso hacer nuevas `queries` sobre la vista, por ejemplo si necesitamos recuperar el email del cliente que ha realizado un pedido (por ejemplo, el pedido #71899):

```sql
-- Recuperar el email al que enviar la confirmación del pedido a partir del SalesOrderID:
SELECT	V.SalesOrderID,
		V.EmailAddress
FROM	SalesLT.vPedidosClientesDireccion V
WHERE	V.SalesOrderID = 71899
```

### SELECT ... SELECT

A partir de la vista anterior, podríamos pensar que el segundo ejemplo (el que hace una `Query` sobre la vista), sabiendo que la vista es una `Query`, podemos hacer algo como esto:

```sql
SELECT	V.SalesOrderID,
		V.EmailAddress
FROM	(
    -- Select de la vista:
    SELECT	O.SalesOrderID,
		C.FirstName,
		C.LastName,
		C.EmailAddress,
		A.AddressLine1,
		A.PostalCode,
		A.City,
		A.StateProvince
    FROM	SalesLT.SalesOrderHeader O
    INNER JOIN SalesLT.Customer C
        ON C.CustomerID = O.CustomerID
    INNER JOIN SalesLT.CustomerAddress CA
        ON CA.CustomerID = C.CustomerID
    INNER JOIN SalesLT.Address A
        ON A.AddressID = CA.AddressID
) as V
WHERE	V.SalesOrderID = 71899
```

De esta forma, podemos trabajar con `subqueries` `SELECT ... SELECT`.

Otro ejemplo, en este caso vamos a realizar un cruce `INNER JOIN` con la subselect:

```sql
SELECT P.ProductID, P.Name, T.Maximo
FROM SalesLT.Product P
INNER JOIN (
	SELECT MAX(D.UnitPrice) as Maximo, D.ProductID
	FROM SalesLT.SalesOrderDetail D
	GROUP BY D.ProductID
) AS T
ON T.ProductID = P.ProductID
```

También podemos utilizar `subqueries` para obtener valores que podamos utilizar en la `query` padre:

```sql
SELECT C.FirstName, COUNT(*)
FROM SalesLT.Customer C
GROUP BY C.FirstName
HAVING COUNT(*) = (
	SELECT MAX(T.Cuenta) FROM (
		SELECT COUNT(*) AS Cuenta
		FROM SalesLT.Customer C2
		GROUP BY C2.FirstName
	) as T
)
```

## Ordenando los resultados

Podemos devolver los resultados ordenados por las columnas que necesitemos, tanto en orden ascendente como descendente, gracias a la cláusula `ORDER BY <columnas>`:

```sql
SELECT	C.CustomerID,
		C.FirstName,
		C.LastName
FROM	SalesLT.Customer C
ORDER BY C.FirstName ASC, C.LastName ASC
```

En este ejemplo, ordenará por la primera columna de forma ascendente, y en caso de que sean iguales, tendrá en cuenta la segunda (y posteriores), como vemos en el resultado:

```
CustomerID	FirstName	LastName
202	        A.	        Leonetti
29943	    A.	        Leonetti
29792	    Abigail	    Gonzalez
345	        Abigail	    Gonzalez
511	        Abraham	    Swearengin
30052	    Abraham 	Swearengin
29702	    Aidan   	Delaney
75	        Aidan	    Delaney
659	        Ajay	    Manchepalli
29978	    Ajay    	Manchepalli
29583	    Alan    	Brewer
148	        Alan	    Brewer
595	        Alan	    Steiner
30031	    Alan    	Steiner
294	        Alberto	    Baltazar
408	        Alexander	Berger
29557	    Alexander	Berger
66	        Alexander	Deborde
29699	    Alexander	Deborde
...
```
