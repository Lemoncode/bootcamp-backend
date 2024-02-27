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
    this.connection!.invoke('SendToTheServer', message);
  }

  receiveMessage() {
    this.connection!.on('SendToTheClient', (message, important) => {

      console.log(`Received message: ${message}`);
      console.log(`Received important: ${important}`);

      this.messages.push(message);
    });
  }

  clear() {
    this.messages = [];
  }
}
