# Añadir notificaciones en tiempo real a una aplicación web con SignalR y Azure Web PubSub

En la última clase del módulo de Azure, vamos a ver cómo añadir notificaciones en tiempo real a una aplicación web con SignalR y Azure Web PubSub.

Pero antes de empezar, vamos a ver qué es SignalR y Azure Web PubSub.

## SignalR

SignalR es una biblioteca de ASP.NET que simplifica la escritura de aplicaciones web en tiempo real. La biblioteca permite a los desarrolladores añadir notificaciones en tiempo real a las aplicaciones web. Lo chulo de esta biblioteca es que se encarga de elegir el mejor mecanismo de transporte disponible, ya sea WebSockets, Server-Sent Events o Long Polling.

Para que puedas probar SignalR hemos dejado dentro del directorio `signalr` todos los componentes que necesitas, esto es:

- `front-end`: nuestra queria aplicación web en Angular con nuestros Tour of Heroes.
- `back-end`: nuestra API en .NET Core que nos permite gestionar los héroes.
- `signalr-api`: Una nueva API en .NET Core que nos permite gestionar las notificaciones en tiempo real.

> **NOTA**: Recuerda que tienes que tener ejecutandose los contenedores de Docker para el SQL Server y para Azurite.


Para ejecutar todo esto, sigue los siguientes pasos:

1. Abre una terminal y navega hasta el directorio `signalr`.
2. Ejecuta el siguiente comando para levantar los contenedores de Docker con SQL Server y Azurite:

```bash
docker start sqlserver azurite
```

3. Ejecuta el siguiente comando para levantar la API de SignalR:

```bash
cd 01-stack-relacional/03-cloud/azure/05-signalr-y-az-webpubsub/signalr/signalr-api
dotnet run --urls=https://localhost:7238
```

4. Ejecuta el siguiente comando para levantar la API:

```bash
cd 01-stack-relacional/03-cloud/azure/05-signalr-y-az-webpubsub/signalr/back-end
dotnet run --urls=http://localhost:5000
```

5. Ejecuta el siguiente comando para levantar la aplicación web:

```bash
cd 01-stack-relacional/03-cloud/azure/05-signalr-y-az-webpubsub/signalr/front-end
npm install && npm start
```

Con todo levantado te darás cuenta de que ahora en la parte inferior de nuestra aplicación podemos ver ciertos mensajes indicando el Id de la conexión del usuario que está generando estos cambios. Esto es gracias a SignalR.

La magia ocurre en primer lugar en el proyecto de la API de SignalR donde tenemos definido un `Hub` que nos permite controlar las funciones del lado del servidor:

```csharp
using Microsoft.AspNetCore.SignalR;

namespace SignalRMessagingTourOfHeroes.Hubs
{
    public class SignalRHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.SendAsync("Send", $"{Context.ConnectionId}: {message}");
            // Clients.Caller.SendAsync("Send", $"{Context.ConnectionId}: {message}");

            // var important = false;

            // if (message.Contains("updated") || message.Contains("added") || message.Contains("delete"))
            // {
            //     important = true;
            //     Clients.All.SendAsync("Send", $"{Context.ConnectionId}: {message}", important);
            // }
            // else
            // {
            //     Clients.Caller.SendAsync("Send", $"{Context.ConnectionId}: {message}", important);
            // }
        }

        public override Task OnConnectedAsync()
        {
            Clients.Others.SendAsync("Send", $"{Context.ConnectionId} joined");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.Others.SendAsync("Send", $"{Context.ConnectionId} left");

            return base.OnDisconnectedAsync(exception);
        }
    }
}
```

En él puedes jugar con diferentes configuraciones como por ejemplo, mandar los mensajes a todos los clientes conectados, al resto (que no seas tú), avisar solo de ciertos mensajes, etc.

Por otro lado, en el lado del cliente, la aplicación web, hemos modificado la implementación del archivo `src/app/message.service.ts` para que se conecte al Hub de SignalR y pueda recibir los mensajes en tiempo real:

```typescript
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root',
})
export class MessageService {

  connection?: signalR.HubConnection;
  messages: string[] = [];

  public initiateSignalrConnection(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(environment.signalRUrl)
        .build();

      //set methods
      this.receiveMessage();

      this.connection.start().then(() => {
        console.log(`SignalR connection success! connectionId: ${this.connection!.connectionId}`);
        resolve();
      }).catch(err => {
        console.log(`SignalR connection error: ${err}`);
        reject(err);
      });
    });
  }


  add(message: string) {
    // this.messages.push(message);
    this.connection!.invoke('Send', message);
  }

  receiveMessage() {
    this.connection!.on('Send', (message, important) => {

      console.log(`Received message: ${message}`);
      console.log(`Received important: ${important}`);

      this.messages.push(message);
    });
  }

  clear() {
    this.messages = [];
  }
}
```

Por el lado de la API que gestiona los héroes no hemos tenido que modificar nada desde la última vez, pero nos hace falta para que la aplicación reciba los héroes ☺️.

Como puedes ver, de una forma súper sencilla hemos añadido notificaciones en tiempo real a nuestra aplicación web 🎉.

Si quisiéramos hacer lo mismo pero con Azure, podríamos usar Azure SignalR Service y para ello solamente tendríamos modificar el archivo Program.cs de la API de SignalR para que se conecte a Azure SignalR Service.

```csharp
builder.Services.AddSignalR().AddAzureSignalR();
```

A este cambio hay que acompañarle con una cadena de conexión en el archivo `appsettings.json`:

```json
{
  "Azure":
  {
    "SignalR": {
        "ConnectionString": "Endpoint=https://<your-azure-signalr-service-name>.service.signalr.net;AccessKey=<your"
    }
  }
}
```

Sin embargo, si no quieres utilizar .NET para desarrollar esta funcionalidad, puedes utilizar Azure Web PubSub.


## Azure Web PubSub

En el caso de Web PubSub, es un servicio completamente administrado que permite a los desarrolladores crear aplicaciones web en tiempo real con WebSockets. Existen diferentes librerías para diferentes lenguajes de programación que te permiten conectarte a este servicio.

Para que puedas probar Azure Web PubSub hemos dejado dentro del directorio `webpubsub` todos los componentes que necesitas, esto es:

- `front-end`: nuestra queria aplicación web en Angular con nuestros Tour of Heroes.
- `back-end`: nuestra API en .NET Core que nos permite gestionar los héroes.
- `webpubsub-az-func`: Para este ejemplo he implementado las llamadas que necesito para este servicio utilizanod Azure Functions y Node.js para que veas que no necesitas .NET para utilizar este servicio. Además he utilizado este artículo de la documentación para implementar la lógica: [Tutorial: Creación de una aplicación de notificación sin servidor con Azure Functions y el servicio Azure Web PubSub.](https://learn.microsoft.com/es-es/azure/azure-web-pubsub/tutorial-serverless-notification?tabs=javascript-v4)

Para arrancar todo esto, sigue los siguientes pasos:

1. Ejecuta el siguiente comando para levantar el proyecto de Azure Functions para Web PubSub:

```bash
cd 01-stack-relacional/03-cloud/azure/05-signalr-y-az-webpubsub/webpubsub/webpubsub-az-func
npm install
func start
```

4. Ejecuta el siguiente comando para levantar la API:

```bash
cd 01-stack-relacional/03-cloud/azure/05-signalr-y-az-webpubsub/webpubsub/back-end
dotnet run --urls=https://localhost:5001
```

5. Ejecuta el siguiente comando para levantar la aplicación web:

```bash
cd 01-stack-relacional/03-cloud/azure/05-signalr-y-az-webpubsub/webpubsub/front-end
npm install && npm start
```

Con todo levantado, podrás ver que la aplicación web sigue funcionando como antes, pero ahora con Azure Web PubSub. En este ejemplo mientras que navegas por la web te irá dando el tiempo ☀️.

Happy coding! 🚀
