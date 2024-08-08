import { APP_INITIALIZER, ErrorHandler, NgModule } from '@angular/core';
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
import { ApplicationinsightsAngularpluginErrorService } from '@microsoft/applicationinsights-angularplugin-js';
import { MonitoringService } from './logging.service';
import { MessageService } from './message.service';


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
    },
    MessageService,
    {
      provide: APP_INITIALIZER,
      useFactory: (messageService: MessageService) => () => messageService.initiateAzWebPubSubConnection(),
      deps: [MessageService],
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

  constructor(private monitoringService: MonitoringService) {

  }
}
