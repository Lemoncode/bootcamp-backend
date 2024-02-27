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