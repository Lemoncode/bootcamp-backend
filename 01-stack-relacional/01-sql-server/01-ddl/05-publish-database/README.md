# Publicar Base de Datos

Partimos del ejemplo anterior: [04-database-project](../04-database-project).

Tenemos creadas las tablas en nuestro proyecto de base de datos, ahora podemos publicar esta estructura en nuestro servidor:

1. Click derecho sobre el nombre del proyecto, y seleccionamos _Publish_:
   ![image-20210821191312690](.\images\publish.png)

2. Nos aparecerá el siguiente cuadro de diálogo, en el que podremos indicar la conexión al servidor donde queremos hacer la publicación (pulsando sobre _Edit_):
   ![image-20210821191525238](.\images\publish-1.png)

3. Al pulsar sobre _Edit_ (en _Target database connection_), se nos abrirá el siguiente cuadro de diálogo donde podemos indicar la conexión a nuestro servidor (en la pestaña _Browse_):
   ![image-20210821191737638](.\images\publish-2.png)

4. Una vez indicada la conexión, configuraremos el nombre de la base de datos destino dentro de nuestro servidor sobre la que queremos hacer la publicación. En nuestro caso, vamos a utilizar TiendaDev, para simular que tenemos un entorno de desarrollo. A continuación guardamos nuestro perfil de publicación seleccionando _Save profile As..._, y lo guardaremos con el nombre **TiendaDev.publish.xml**:

   ![image-20210821192213826](.\images\publish-3.png)

5. Con esto se añadirá en la raíz del proyecto nuestro perfil de publicación para desarrollo:
   ![image-20210821192441524](.\images\publish-4.png)

6. Ahora podemos generar el _script_ para publicar nuestra base de datos (pulsando sobre _Generate Script_) para ejecutarlo manualmente o revisar los cambios que se van a ejecutar, o publicarla en nuestro servidor con _Publish_.

7. Al pulsar sobre _Publish_ se creará la nueva base de datos TiendaDev con toda la estructura en nuestro servidor SQL Server. Podemos verla desde SSMS o Azure Data Studio:

   ![image-20210821195604249](.\images\database-published.png)

## Cambios de estructura

Ahora, cuando realicemos cambios sobre la estructura de nuestra base de datos (por ejemplo editar tablas o crear nuevas), el perfil de publicación detectará los cambios entre el código del proyecto y la base de datos donde va a realizar la publicación, y generará el _script_ con los cambios o los ejecutará si seleccionamos _Publish_.

Para ver un ejemplo, en el proyecto de base de datos, vamos a añadir una nueva columna en la tabla _Pedido_ que llamaremos CuponDescuentoId:

![image-20210821193512132](.\images\changes-1.png)

A continuación volvemos a ejecutar nuestro perfil de publicación _TiendaDev_ y antes de publicar generamos el _script_ para revisar los cambios. Como vemos, los cambios que va a ejecutar sobre la base de datos sólo afectan a la nueva columna que hemos creado:

```sql
PRINT N'Altering [dbo].[Pedido]...';


GO
ALTER TABLE [dbo].[Pedido]
    ADD [CuponDescuentoId] INT NULL;


GO
PRINT N'Update complete.';
```

Al publicar sobre nuestro servidor, veremos que los cambios se hacen efectivos sobre la base de datos _TiendaDev_.

## Crear otro perfil para Producción

Siguiendo el mismo proceso, creamos otro perfil para el entorno de producción (por ejemplo, _TiendaProd_). Veremos que al generar el _script_, en este caso, contendrá todo lo relacionado con crear la BBDD, crear las tablas, etc. ya que no existe y es la primera vez que publicamos en este entorno.

En el proceso de publicación, se compara la estructura del código fuente de nuestro proyecto, con la estructura existente en la base de datos donde vamos a publicar, y se obtiene el script con los cambios necesarios.

Esta forma de trabajar es muy útil y potente a la hora de manejar distintos entornos y los cambios que debemos realizar en cada uno cuando publicamos nuevas versiones de nuestro producto.
