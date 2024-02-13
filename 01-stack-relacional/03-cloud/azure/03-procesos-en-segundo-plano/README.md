# Procesos en segundo plano en Microsoft Azure

Ahora que ya has desplegado tu primera aplicación y que sabes cómo almacenar las imágenes de tus héroes en Azure Storage, vamos a ver cómo ejecutar procesos en segundo plano que trabajen con estos assets.

Pero antes de nada, **¿qué es un proceso en segundo plano?** Un proceso en segundo plano es un programa que se ejecuta en el sistema operativo sin interacción con el usuario. En el caso de una aplicación web, un proceso en segundo plano puede ser útil para realizar tareas que no necesitan ser ejecutadas en el contexto de una petición HTTP, como por ejemplo, enviar correos electrónicos, procesar imágenes, o realizar tareas de mantenimiento.

Para que puedas probar cómo ejecutar estos procesos en segundo plano en diferentes servicios, y que no te suponga un coste adicional, vamos a utilizar **Azurite**. [Azurite](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio%2Cblob-storage) es un emulador de Azure Storage que puedes ejecutar en tu máquina local. De esta forma, no necesitarás una cuenta de Azure para probar los ejemplos de este capítulo 🥳.

Puedes utilizar tanto la extensión de [Azurite para Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=Azurite.azurite), como la imagen de Docker, ahora que ya aprendiste Docker... ¡en 3 horas 😅!


```bash
docker run \
--name azurite \
-p 10000:10000 \
-p 10001:10001 \
mcr.microsoft.com/azure-storage/azurite
```
Para subir a este emulador las imágenes, de los héroes y de los alter egos, puedes utilizar el mismo comando que vimos en la sesión anterior:

Para los heroes:

```bash
az storage blob upload-batch \
--destination heroes \
--source 01-stack-relacional/03-cloud/azure/03-procesos-en-segundo-plano/assets/heroes/. \
--connection-string "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
```

Para los alter egos:

```bash
az storage blob upload-batch \
--destination heroes \
--source 01-stack-relacional/03-cloud/azure/03-procesos-en-segundo-plano/assets/alteregos/png/. \
--connection-string "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
```

Para esta temática, se ha actualizado tanto el proyecto de Angular como la API en .NET para que seamos capaces de subir imágenes a Azure Storage. Puedes encontrar el código en la carpeta `front-end` y `back-end` respectivamente. ¡Vamos a probarlo!

Antes de nada asegurate que el contenedor de SQL server que creamos en el primer capítulo está en ejecución. Si no es así, puedes ejecutarlo con el siguiente comando:

```bash
docker start sqlserver
```

Ejecuta la API utilizando estos comandos:

```bash
cd 01-stack-relacional/03-cloud/azure/03-procesos-en-segundo-plano/back-end
dotnet run
```

En esta versión nuestra API ya viene configurada para trabajar con Azurite. Lo único que se ha tenido que modificar es la cadena de conexión a **UseDevelopmentStorage=true**.

```json{5}
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost,1433;Initial Catalog=heroes;Persist Security Info=False;User ID=sa;Password=Password1!;Encrypt=False",
        "AzureStorage": "UseDevelopmentStorage=true"
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "AllowedHosts": "*"
}
```
Con ello el SDK ya sabe que tiene que comunicarse con Azurite en lugar de con Azure Storage 🥳.

Por otro lado, se ha añadido un método adicional llamado `GetAlterEgoPicSas`. Este nos permitirá recuperar una clave temporal que le permita a nuestra app en angular subir imágenes para los alter egos.

```csharp
        // GET: api/hero/alteregopic/sas
        [HttpGet("alteregopic/sas/{imgName}")]
        public ActionResult GetAlterEgoPicSas(string imgName)
        {
            //Get image from Azure Storage
            string connectionString = _configuration.GetConnectionString("AzureStorage");

            // Create a BlobServiceClient object which will be used to create a container client
            var blobServiceClient = new BlobServiceClient(connectionString);

            //Get container client
            var containerClient = blobServiceClient.GetBlobContainerClient("alteregos");

            //Get blob client
            var blobClient = containerClient.GetBlobClient(imgName);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = "alteregos",
                BlobName = imgName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(3)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);

            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

            Console.WriteLine($"SAS Uri for blob is: {sasUri}");

            //return image
            return Ok($"{blobServiceClient.Uri}{sasUri.Query}");
        }    
    }
```


Ejecuta el front-end utilizando estos comandos:

```bash
cd 01-stack-relacional/03-cloud/azure/03-procesos-en-segundo-plano/front-end
npm install
npm start
```

Si intentas subir una imágen de un alter ego te dará error.

<img src="images/Error de CORS de Azurite.png" />

Esto es porque aunque tengas una clave temporal es necesario que la cuenta de almacenamiento tenga configurado CORS. Para ello puedes ejecutar el siguiente comando:

```bash
az storage cors add \
--methods GET POST PUT DELETE \
--origins '*' \
--services b \
--max-age 60 \
--allowed-headers '*' \
--exposed-headers '*' \
--connection-string "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
```
Ahora intenta de nuevo a modificar la imágen 🙃

¡Genial ahora que ya tienes la nueva versión de Tour Of Heroes funcionando con Azurite, vamos a empezar a investigar formas de ejecutar procesos en segundo plano! 🚀

# Procesos en segundo plano con Azure Functions

## ¿Qué es Azure Functions?

Azure Functions es un servicio de cómputo sin servidor que te permite ejecutar código en respuesta a eventos sin tener que preocuparte por la infraestructura. Puedes utilizar Azure Functions para ejecutar un script o pieza de código en respuesta a una variedad de eventos. Estos eventos pueden ser desencadenados por cambios en datos, la ejecución de un cronograma, interacciones con un servicio HTTP, o la recepción de mensajes en una cola, entre otros.

En este ejemplo vamos a hacer una Azure Function, también en local, que nos van a permitir arreglar un problema que ahora mismo tiene mi aplicación: y es que si cambio la imagen de un alter ego en formato jpeg, la aplicación no la va a poder mostrar, ya que solo entiende de imágenes en formato png 🤔. Por lo que vamos a crear un método para este servicio que si detecta que subimos un jpeg lo convierta a png para que mi aplicación siga funcionando.

## Crear una Azure Function

Para crear una Azure Function en local, necesitamos instalar el Azure Functions Core Tools. Puedes hacerlo con el siguiente comando:

```bash
npm i -g azure-functions-core-tools@4 --unsafe-perm true
```

Una vez instalado, vamos a crear un nuevo proyecto de Azure Functions. Para ello, ejecuta el siguiente comando:

```bash
mkdir -p 01-stack-relacional/03-cloud/azure/03-procesos-en-segundo-plano/azure-functions
cd 01-stack-relacional/03-cloud/azure/03-procesos-en-segundo-plano/azure-functions
func init
```
El último comando iniciará un asistente donde tenemos que elegir el lenguaje de programación con el que queremos desarrollar esta Azure Function. Para este ejemplo elegiremos `1.dotnet` y en el segundo paso el lenguaje será `1.c#`. Una vez finalice la creación la misma estará disponible en la carpeta `01-stack-relacional/03-cloud/azure/03-procesos-en-segundo-plano/azure-functions`.

```bash
cd 01-stack-relacional/03-cloud/azure/03-procesos-en-segundo-plano/azure-functions
```

¡Pero esto es solo el proyecto! ahora lo que hace falta es crear las funciones como tal. Para este ejemplo vamos a crear una que escuche nuestra cuenta de almacenamiento, en este caso Azurite, y que si detecta en el contenedor alter egos que se sube una imagen en formato jpeg, la convierta a png.

Para ello, ejecuta el siguiente comando:

```bash
func new
```
Ahora en el asistente elige la opción `3. BlobTrigger` y dale un nombre a la función, por ejemplo `ConvertImageToPng`. En el siguiente paso elige el lenguaje de programación, en este caso `1. C#`. Por último, elige el nombre del contenedor que quieres que escuche, en este caso `alteregos`. Una vez generada verás que dentro del directorio `azure-functions` tenemos un nueva clase llamada `ConvertImageToPng.cs`.

Reemplaza el contenido por el siguiente:

```csharp
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace azure_functions
{
    public class ConvertImageToPng
    {
        [FunctionName("ConvertImageToPng")]
        public void Run([BlobTrigger("alteregos/{name}.jpeg", Connection = "AzureStorageConnection")] Stream myBlob, string name, ILogger log,
            [Blob("alteregos/{name}.png", FileAccess.Write, Connection = "AzureStorageConnection")] Stream outputBlob)
        {            
            log.LogInformation($"Converting {name}.jpeg to {name}.png");
            
            using (var image = Image.Load(myBlob))
            {
                image.SaveAsPng(outputBlob);
            }          
        }
    }
}
```

Como ves, la función es muy sencilla: escucha el contenedor `alteregos` y si detecta que se sube una imagen en formato jpeg, la convierte a png. Para ello utiliza la librería `SixLabors.ImageSharp` que es una librería de manipulación de imágenes de alto rendimiento y fácil de usar para .NET.

Para añadir la referencia a esta librería, ejecuta el siguiente comando:

```bash
dotnet add package SixLabors.ImageSharp
```

y añadir la configuración en `local.settings.json` para que pueda conectarse con Azurite:

```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "AzureStorageConnection": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet"
    }
}
```

Ahora que ya tenemos la función, vamos a ejecutarla. Para ello, ejecuta el siguiente comando:

```bash
func start
```

Si todo ha ha ido bien deberías de ver algo como lo siguiente en el terminal:

<img src="images/Azure Function con BlobTrigger ejecutandose.png" />

Ahora la prueba de fuego 🔥 sería subir una imagen al contenedor de alter egos y comprobar que efectivamente esta función se ejecuta y que tenemos en el propio contenedor el resultado guardado en png. Para ello, elimina todas las imagenes que hay en el contenedor con Azure Storage Explorer y utiliza las imágenes guardadas en `assets/alteregos/jpeg`.

También puedes probar desde la interfaz en Angular.