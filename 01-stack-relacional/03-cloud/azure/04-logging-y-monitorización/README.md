# Monitorización de aplicaciones en Azure

## Introducción

En este laboratorio vamos a ver cómo monitorizar aplicaciones en Azure. Para ello, vamos a utilizar Azure Monitor, que es una solución de monitorización y análisis de datos que nos permite obtener información sobre el rendimiento y la disponibilidad de nuestras aplicaciones. Dentro de este servicio, vamos a ver cómo utilizar Application Insights, que es una herramienta que nos permite monitorizar el rendimiento y el uso de nuestras aplicaciones.

Podemos monitorizar diferentes tipos de aplicaciones, como aplicaciones web, aplicaciones móviles, aplicaciones de escritorio, servicios web, etc. Además, podemos monitorizar aplicaciones que se ejecutan en diferentes entornos, como máquinas virtuales, contenedores, servicios de Azure, etc. Para esta clase vamos a ver cómo se puede monitorizar nuestra API y nuestro frontend.

Las nuevas versiones de las mismas están en los directorios `back-end` y `front-end` respectivamente.

Para ejecutar la API puedes seguir estos pasos:

```bash
cd 01-stack-relacional/03-cloud/azure/04-logging-y-monitorización/back-end
```

A esta nueva versión se le han añadido varias cosas. La primera de ellas una nueva librería llamada `Microsoft.ApplicationInsights.AspNetCore` la cual nos permite de forma sencilla instrumentalizar nuestra aplicación. Para ello lo único que hemos tenido que hacer, además de añadir esta librería ha sido añadir como parte del archivo appsettings.json la siguiente configuración:

```json
{
  "ApplicationInsights": {
    "ConnectionString": "YOU_CONNECTION_STRING"
  }
}
```

Para recuperar ese valor debemos crear un recurso de Application Insights bien desde el portal de Azure o bien utilizando el siguiente comando:

```bash
RESOURCE_GROUP="tour-of-heroes"
APP_NAME="tour-of-heroes"
API_NAME="tour-of-heroes-api"
LOCATION="westeurope"

# Create a Log Analytics workspace
az monitor log-analytics workspace create \
--resource-group $RESOURCE_GROUP \
--workspace-name $APP_NAME-log-analytics \
--location $LOCATION

# Get Log Analytics workspace id
LOG_ANALYTICS_WORKSPACE_ID=$(az monitor log-analytics workspace show \
--resource-group $RESOURCE_GROUP \
--workspace-name $APP_NAME-log-analytics \
--query "id" -o tsv)

# Create Application Insights
az monitor app-insights component create \
--app $API_NAME-insights --location $LOCATION \
--kind web -g $RESOURCE_GROUP \
--application-type web \
--workspace $LOG_ANALYTICS_WORKSPACE_ID
```

Y por otro lado hemos añadido al archivo `Program.cs` la siguiente línea:

```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

Solo con esto ya tenemos nuestra aplicación monitorizada. Si queremos ver los datos que se están recogiendo, podemos ir a la consola de Azure y buscar Application Insights. Una vez dentro, podremos ver todas las métricas que se están recogiendo.

## Intrumentalización de una aplicación Angular

> [!NOTE] Si quieres ver las imágenes *artísticas* en el Dashboard de los héroes en tu local puedes subir la carpeta `artistic`en el directorio `assets`de esta unidad, para que no dependas ni de la Logic App de la unidad anterior ni de una cuenta de Azure Storage.

Desde el punto de vista del frontal web hemos tenido que instalar estos dos paquetes de npm:

```bash
npm install @microsoft/applicationinsights-web @microsoft/applicationinsights-angularplugin-js
```
Y modificar el archivo `app.module.ts` para añadir la siguiente configuración:

```typescript
import { ErrorHandler, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HeroesComponent } from './heroes/heroes.component';

//Import FormsModule to use ngModel
import { FormsModule } from '@angular/forms';
import { HeroDetailComponent } from './hero-detail/hero-detail.component';
import { MessagesComponent } from './messages/messages.component';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';

import { HttpClientModule } from '@angular/common/http';

import { HeroSearchComponent } from './hero-search/hero-search.component';
import { ReplacePipe } from './replace.pipe';
import { FileUploadComponent } from './file-upload/file-upload.component';

//Monitoring with Application Insights
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { AngularPlugin, ApplicationinsightsAngularpluginErrorService } from '@microsoft/applicationinsights-angularplugin-js';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { MonitoringService } from './logging.service';


@NgModule({
  declarations: [
    AppComponent,
    HeroesComponent,
    HeroDetailComponent,
    MessagesComponent,
    DashboardComponent,
    HeroSearchComponent,
    ReplacePipe,
    FileUploadComponent //A pipe for replace characters
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,    
  ],
  providers: [
    {
      provide: ErrorHandler,
      useClass: ApplicationinsightsAngularpluginErrorService
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

  constructor(private monitoringService: MonitoringService) {
  }
}
```

Lo más importante de todo esto es que hemos añadido un nuevo servicio llamado `MonitoringService` que nos permite monitorizar nuestra aplicación. Este servicio es el siguiente:

```typescript
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AngularPlugin } from '@microsoft/applicationinsights-angularplugin-js';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { environment } from 'src/environments/environment';

@Injectable(
    { providedIn: 'root' }
)
export class MonitoringService {
    appInsights: ApplicationInsights;
    angularPlugin: AngularPlugin;

    constructor(private router: Router) {

        this.angularPlugin = new AngularPlugin();

        this.appInsights = new ApplicationInsights({
            config: {
                instrumentationKey: environment.appInsights.instrumentationKey,                
                extensions: [this.angularPlugin],
                extensionConfig: {
                    [this.angularPlugin.identifier]: { router: this.router }
                }
            }
        });

        this.appInsights.loadAppInsights();
    }

    logEvent(name: string, properties?: { [key: string]: any }) {
        this.appInsights.trackEvent({ name: name }, properties);
    }

    logMetric(name: string, average: number, properties?: { [key: string]: any }) {
        this.appInsights.trackMetric({ name: name, average: average }, properties);
    }

    logException(exception: Error, severityLevel?: number) {
        this.appInsights.trackException({ exception: exception, severityLevel: severityLevel });
    }

    logTrace(message: string, properties?: { [key: string]: any }) {
        this.appInsights.trackTrace({ message: message }, properties);
    }
}
```

En esta clase he definido los diferentes tipos de trazas que podemos enviar a Application Insights. En este caso, he definido eventos, métricas, excepciones y trazas. Para enviar un evento, por ejemplo, solo tendríamos que hacer lo siguiente:

Se puede ver claramente su uso en el archivo `src/hero.service.ts`:

```typescript
import { Injectable } from '@angular/core';
import { Hero } from './hero';
import { Observable, of } from 'rxjs';
import { MessageService } from './message.service';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { MonitoringService } from './logging.service';

@Injectable({
  providedIn: 'root',
})
export class HeroService {

  private heroesUrl = environment.apiUrl;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private messageService: MessageService, private http: HttpClient, private monitoringService: MonitoringService) { }

  private log(message: string) {
    this.messageService.add(`HeroService: ${message}`);
  }

  getSasToken(imageName: string): Observable<any> {
    const headers = new HttpHeaders().set('Content-Type', 'text/plain; charset=utf-8');

    return this.http.get(`${this.heroesUrl}/alteregopic/sas/${imageName}`,
      { headers: headers, responseType: 'text' })
      .pipe(
        tap(_ => this.log('get sas token to upload image')),
        catchError(this.handleError<Hero[]>('getSasToken', [])));;
  }

  getAlterEgoPic(id: number): Observable<Blob> {
    return this.http.get(`${this.heroesUrl}/alteregopic/${id}`, { responseType: 'blob' });
  }

  getHeroes(): Observable<Hero[]> {
    // const heroes = of(HEROES);
    // this.messageService.add('HeroService: fetched heroes')
    // return heroes;

    return this.http.get<Hero[]>(this.heroesUrl).pipe(
      tap((_) => this.log('fetched heroes')),
      catchError(this.handleError<Hero[]>('getHeroes', [])),
    );
  }

  /** GET hero by id. Will 404 if id not found */
  getHero(id: number): Observable<Hero> {
    // const hero = HEROES.find(h => h.id === id)!;
    // this.messageService.add(`HeroService: fetched hero id=${id}`);
    // return of(hero);

    const url = `${this.heroesUrl}/${id}`;
    return this.http.get<Hero>(url).pipe(
      tap((_) => this.log(`fetched hero id=${id}`)),
      catchError(this.handleError<Hero>(`getHero id=${id}`)),
    );
  }

  /** PUT: update the hero on the server */
  updateHero(hero: Hero): Observable<any> {
    // Create the route - getting 405 Method not allowed errors
    const url = `${this.heroesUrl}/${hero.id}`;

    return this.http.put(url, hero, this.httpOptions).pipe(
      tap(_ => {
        this.log(`updated hero id=${hero.id}`);
        this.monitoringService.logEvent('updateHero', { "name": hero.name })
      }),
      catchError(this.handleError<any>('updateHero'))
    );
  }

  /** POST: add a new hero to the server */
  addHero(hero: Hero): Observable<Hero> {
    return this.http.post<Hero>(this.heroesUrl, hero, this.httpOptions).pipe(
      tap((newHero: Hero) => {
        this.log(`added hero w/ id=${newHero.id}`);
        this.monitoringService.logEvent('addHero', { "name": newHero.name });
      }),
      catchError(this.handleError<Hero>('addHero'))
    );
  }

  /** DELETE: delete the hero from the server */
  deleteHero(id: number): Observable<Hero> {
    const url = `${this.heroesUrl}/${id}`;

    return this.http.delete<Hero>(url, this.httpOptions).pipe(
      tap(_ => {
        this.log(`delete hero id=${id}`);
        this.monitoringService.logEvent('deleteHero', { "id": id });
      }),
      catchError(this.handleError<Hero>('deleteHero'))
    )
  }

  /* GET heroes whose name contains search term */
  searchHeroes(term: string): Observable<Hero[]> {
    if (!term.trim()) {
      //if not search term, return empty hero array.
      return of([]);
    }
    return this.http.get<Hero[]>(`${this.heroesUrl}/?name=${term}`).pipe(
      tap((x) =>
        x.length
          ? this.log(`found heroes matching "${term}"`)
          : this.log(`no heroes matching "${term}"`),
      ),
      catchError(this.handleError<Hero[]>('searchHeroes', [])),
    );
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      //TODO: send the error to remote logging infrastructure
      console.error(error); //log to console instead

      //TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      //Let the app keep running by returning an empty result
      return of(result as T);
    };
  }
}
```

En este caso la clave que se necesita para conectarse al recurso que generemos de Application Insights se encuentra en el archivo `src/environments/environment.ts`:

```typescript
  appInsights: {
    instrumentationKey: "9ff425c5-0949-4e13-9970-fcd4066ce274"
  },
```

Igualmente podemos crear este recurso a través de la línea de comandos o a través del portal.

Ejecuta el siguiente comando para arrancar el frontal web:

```bash
cd 01-stack-relacional/03-cloud/azure/04-logging-y-monitorización/front-end
npm run start
```

## Bonus track 🎉: OpenTelemetry

OpenTelemetry es un proyecto de código abierto que proporciona una forma de instrumentar aplicaciones para recopilar métricas, trazas y logs. OpenTelemetry es una combinación de OpenCensus y OpenTracing, dos proyectos de código abierto que se unieron para formar OpenTelemetry.
El objetivo de OpenTelemetry es proporcionar una forma de instrumentar aplicaciones para recopilar métricas, trazas y logs. OpenTelemetry proporciona una API unificada para instrumentar aplicaciones y enviar datos a diferentes proveedores de telemetría. Lo cual nos permite enviar datos a diferentes proveedores de telemetría sin tener que cambiar el código de la aplicación.

Por un lado tenemos varias librerías instaladas en nuestro proyecto que puedes ver en el archivo `tour-of-heroes-api.csproj`. Y por otro esta configuración en el archivo `Program.cs`:

```csharp
/************************************************************************************************
********************************** Open Telemetry configuration *********************************
*********** https://grafana.com/grafana/dashboards/17706-asp-net-otel-metrics/ ******************
************************************************************************************************/

string serviceName = builder.Configuration["OTEL_SERVICE_NAME"] ?? "tour_of_heroes_api";

builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeScopes = true;
    options.IncludeFormattedMessage = true;

    var resourceBuilder = ResourceBuilder.CreateDefault();
    resourceBuilder.AddService(serviceName);
    options.SetResourceBuilder(resourceBuilder);

    options.AddConsoleExporter();
    options.AddOtlpExporter(); //This will, by default, send traces using gRPC to http://localhost:4317

});

builder.Services.AddHttpLogging(o => o.LoggingFields = HttpLoggingFields.All);

builder.Services.AddOpenTelemetry()
.UseAzureMonitor() //https://learn.microsoft.com/es-es/azure/azure-monitor/app/opentelemetry-configuration?tabs=aspnetcore
.ConfigureResource(resource => resource.AddService(serviceName))
.WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation();
    tracing.AddHttpClientInstrumentation();
    // tracing.AddSqlClientInstrumentation();
    tracing.AddEntityFrameworkCoreInstrumentation();
    
    tracing.AddOtlpExporter();

    tracing.AddConsoleExporter();

})
.WithMetrics(metrics =>
{
    metrics.AddAspNetCoreInstrumentation();
    metrics.AddHttpClientInstrumentation();
    metrics.AddProcessInstrumentation();
    metrics.AddRuntimeInstrumentation();
    metrics.AddConsoleExporter();
    
    // https://opentelemetry.io/docs/instrumentation/net/exporters/#prometheus-experimental
    metrics.AddPrometheusExporter();

    metrics.AddOtlpExporter();

});

/************************************************************************************************/
```

Este código define a dónde se mandan las métricas, las trazas y los logs. En este caso se mandan a la consola y a un servidor de OpenTelemetry que se encuentra en `http://localhost:4317`. Para poder hacer este ejemplo lo más real posible, he creado una configuración de Dev Containers donde además de tener el proyecto de la API, tengo un servidor de OpenTelemetry que recoge los datos y los envía a diferentes proveedores de telemetría: Los logs a Loki, las métricas a Prometheus y las trazas a Jaeger. Y además de todo lo anterior a Azure Monitors

Si te gustaría saber más sobre ello puedes ver mi vídeo en Youtube:

[![alt text](img/Video%20sobre%20Continuos%20monitoring%20y%20OpenTelemetry.png "Title")](https://www.youtube.com/watch?v=a6l-KlPVpFI&list=PLO9JpmNAsqM7-LFcEdOJMZOGs5kdUE2PB)