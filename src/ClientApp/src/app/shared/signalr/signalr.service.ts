import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubPath = environment.apiUrl + "/text-conversion-hub"
  private hubConnection: HubConnection | undefined;
  private messageSubject$ = new Subject<string>();
  receivedMessage$ = this.messageSubject$.asObservable();

  public listen() {
    if (this.hubConnection) {
      (this.hubConnection).on("ConversionResult", (data: string) => {
        console.log(data);
        this.messageSubject$.next(data);
      });
    }
  }

  public joinGroup(groupName: string) {
    return new Promise((resolve, reject) => {
      if (this.hubConnection) {
        this.hubConnection
          .invoke("AddToGroupAsync", groupName)
          .then(() => {
            console.log("added to group");
            return resolve(true);
          }, (err: any) => {
            console.log(err);
            return reject(err);
          });
      } else {
        return reject();
      }
    })
  }

  public startConnection() {
    return new Promise((resolve, reject) => {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(this.hubPath)
        .build();

      this.hubConnection.start()
        .then(() => {
          console.log("connection established");
          return resolve(true);
        })
        .catch((err: any) => {
          console.log("error occured" + err);
          reject(err);
        });
    });
  }

  constructor() { }
}