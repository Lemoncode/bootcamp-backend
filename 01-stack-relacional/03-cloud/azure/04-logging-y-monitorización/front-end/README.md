[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/0gis0/tour-of-heroes-angular)

# Aplicación de ejemplo en Angular: Tour Of Heroes

En esta versión del proyecto hemos instrumentalizado el mismo con Application Insights. Para ello hemos instalado dos nuevos paquetes:

```
npm install @microsoft/applicationinsights-web @microsoft/applicationinsights-angularplugin-js
```

## Cambios

Para poder hacer uso de estas librerías se ha modificado el archivo ***src/app/app.module.ts*** donde en el constructor se ha inyectado una nueva clase llamada ***MonitoringService***:

```
constructor(private monitoringService: MonitoringService) {

}
```

y se ha implementado esta de la siguiente manera:

````
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
````

***Nota***: como se puede ver en esta, es necesario guardar en el archivo ***environments/environment.ts*** el valor de la instrumentation key del servicio que podemos crear usando este comando:

```
az monitor app-insights component create --app angular-tour-of-heroes --location northeurope -g Lemoncode-Tour-Of-Heroes
```
