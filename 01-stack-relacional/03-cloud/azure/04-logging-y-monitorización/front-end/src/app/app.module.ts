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

//In memory web api
// import { HttpClientInMemoryWebApiModule } from 'angular-in-memory-web-api';
// import { InMemoryDataService } from './in-memory-data.service';
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
    // HttpClientInMemoryWebApiModule.forRoot(InMemoryDataService, { dataEncapsulation: false })
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

  // constructor(private router: Router) {
  //   var angularPlugin = new AngularPlugin();

  //   const appInsights = new ApplicationInsights({
  //     config: {
  //       instrumentationKey: environment.appInsights.instrumentationKey,
  //       extensions: [angularPlugin],
  //       extensionConfig: {
  //         [angularPlugin.identifier]: { router: this.router }
  //       }
  //     }
  //   });

  //   appInsights.loadAppInsights();

  // }

  constructor(private monitoringService: MonitoringService) {

  }
}
