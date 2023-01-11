# Manual de despliegue en Azure
En este ejemplo vamos a aprender cómo crear y desplegar un sitio web compuesto por una Api y un frontal desarrollado con React a servicios en Azure
In this example we are going to learn how to create and deploy a website composed by an Api and a React site to Azure.

## Proyecto
Podemos usar el proyecto de ejemplo que se provee en la carpeta 00-demo-project o crear los proyectos de Api y JavaScript. 
> Es válido usar los proyectos generados en anteriores capítulos de este bootcamp

## Crear proyectos
Si hemos decidido optar por crear nuestros propios proyectos, en el caso de 

### Crear proyecto ASP.Net Core Api
En el caso del proyecto de Api, tenemos dos opciones, desde el IDE Visual Studio o desde línea de comandos.
Si queréis más información al respecto, podéis visitar el enlace <https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio>

#### Desde Visual Studio
La manera más sencilla de crear el proyecto, es realizarlo desde Visual Studio.
- Desde el inicio al abrir Visual Studio o desde el menú Archivo, seleccionar Nuevo > proyecto
- Poner Api en la caja de búsqueda
- En la configuración del nuevo proyecto, darle un nombre, en esta demo se le ha puesto Lemoncode.Azure.Api
- En la información adicional
    - Confirmar el Framework que se va a usar. En esta demo es .NET 6.0 (o superior)
    - Confirmar usar controladores o no con el check correspondiente. Si se quita el check, se usará *Mínimal Api (no recomendable aún para proyectos complejos en producción)*
    - Seleccionar Crear

#### Desde línea de comandos
Para crear el proyecto de Api desde línea de comandos tenemos que ir hasta la carpeta donde queremos crear el proyecto y escribir los siguientes comandos.

    dotnet new webapi -o TodoApi
    cd TodoApi

Añadir EntityFramework (Opcional):

    dotnet add package Microsoft.EntityFrameworkCore.InMemory
    code -r ../TodoApi

### Crear proyecto React

El proyecto provisto en la demo se ha creado mediante *create-react-app* usando la plantilla con *TypeScript*.

Para realizar la creación, hay que ejecutar el siguiente comando.

    npx create-react-app "nombre_proyecto" --template typescript

Si queréis más información, podéis visitar el enlace <https://create-react-app.dev/docs/getting-started/>

## Despliegue de proyectos
### Despliegue de Api
Para desplegar la api tenemos diferentes métodos. El más simple es desde el propio Visual Studio, desde el que, además podemos aprovisionar los recursos necesarios para nuestra aplicación. No obstante, lo más recomendado es realizarlo desde un repositorio de código, usando pipelines.
#### Despliegue desde Visual Studio.
En este caso, nos es indiferente que existan ya los recursos en Azure ya que, el proceso de despliegue desde Visual Studio es capaz de crearlos.
Los pasos para realizar el despliegue son:
- Click con el botón derecho sobre el nombre del proyecto que queremos desplegar. Esto abrirá el menú contextual donde se encuentra la acción ***Publicar***
- Seleccionar la acción ***Publicar***, lo que abrirá una ventana que nos mostrará los pasos para realizar la publicación de la aplicación
- En la lista *Target*, seleccionar **Azure** y **Siguiente**
- En este momento, se muestra el tipo de recurso que queremos desplegar, entre ellos, *Azure App Service [Windows/Linux], Azure Container Apps o Virtual Machine*. Seleccionamos uno de los dos tipos de **App Service**. Recomiendo Windows en este caso inicial, aunque tiene mayor coste, pero dispone de algunas opciones más de configuración.
- En el siguiente paso, se solicita seleccionar la suscripción donde queremos realizar el despliegue
- Una vez iniciada la sesión en la suscripción, aparece el listado de recursos que coincida con el seleccionado anteriormente, *App Service Windows*, por si quisiéramos desplegar en uno ya existente. De no ser así, seleccionar **+ Crear nuevo**
- Aparecerá un formulario que nos solicita el nombre que tendrá el app service. 
    - Name: Es importante que tengamos claro que debe ser un nombre único porque se usará para la url que será del tipo *[APP_SERVICE_NAME].azurewebsites.net*. 
    - Nombre de suscripción: Seleccionar la suscripción adecuada.
    - Grupo de recursos: Es el nombre del grupo/contenedor donde se agruparán los recursos que creemos. Como si fuera una carpeta en nuestro PC. Podemos seleccionar uno existente o crear un nuevo desde el formulario que se abre al seleccionar *Nuevo*
    - Hosting Plan: Podemos seleccionar un plan de pago existente si quisiéramos agrupar servicios en un plan (_cuidado con los límites_) o crear uno nuevo seleccionando *Nuevo*, lo que abriría otro formulario donde seleccionar el nombre del plan, la ubicación y el plan de precios (tamaño) que queremos usar.
    - Seleccionamos **Crear** y se inicia el proceso de creación de los recursos.
- Una vez creados los recursos, *App Service y App Service Plan*, se cierra el formulario, apareciando seleccionado el servicio que acabamos de crear. Seleccionamos **Siguiente**
- En este paso, podremos aprovisionar un Api Management si quisiéramos. Omitimos este paso por el momento, seleccionando *Skip this step* y seguidamente, seleccionamos **Finalizar** y cerramos la ventana de resumen donde se indica que se ha creado un perfil de publicación.
- Llegados a este punto, la ventana de publicación ha camiado un poco y nos aparece como *Listo para publicar*. Si atendemos a la zona inferior de la ventana, podemos ver que nos aparecen servicios que podríamos crear desde este punto como_
    - Api Management
    - SQL Server Database
    - Cuenta de almacenamiento de Azure
- Para finalizar, en la parte superior de la ventana, seleccionamos **Publicar** y esperamos el resultado. Es importante atender a la ventana de depuración del despliegue que se abre en la parte baja de Visual Studio.
