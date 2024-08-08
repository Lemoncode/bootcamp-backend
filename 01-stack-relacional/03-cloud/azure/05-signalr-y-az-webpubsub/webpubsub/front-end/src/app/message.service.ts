import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MessageService {

  ws?: WebSocket;
  messages: string[] = [];

  public initiateAzWebPubSubConnection(): Promise<void> {
    return new Promise(async (resolve, reject) => {
      let res = await fetch('http://localhost:7071/api/negotiate');
      let url = await res.json();
      console.log(`url: ${url.url}`);
      this.ws = new WebSocket(url.url,"json.webpubsub.azure.v1");

      this.ws.onopen = () => {
        console.log('connection opened');

        this.ws!.send(JSON.stringify(
          {
            from: 'me',
            type: 'joinGroup',
            group: 'group1',
          }
        ));
        
      }

      this.ws.onclose = e => console.log(`connection closed (${e.code})`);

      this.ws.onerror = e => {
        console.log('error');
        console.log(e);
      };

      this.receiveMessage();

      resolve();

    });
  }


  add(message: string) {
    if (this.ws?.readyState === WebSocket.OPEN) {
      console.log(`Sending message: ${message}`);

      this.ws!.send(JSON.stringify(
        {
          from: 'me',
          type: 'sendToGroup',
          dataType: 'text',
          group: 'group1',
          noEcho: false,
          data: message,
        }
      ));


      // this.ws!.send(message);
    }
  }

  receiveMessage() {
    this.ws!.onmessage = (e) => {

      if (!e.data) return;

      console.log(`Received message: ${e.data}`);
      var server_message = e.data;
      console.log(server_message);
      this.messages.push(server_message);
    }
  }

  clear() {
    this.messages = [];
  }
}
