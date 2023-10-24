import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubPath = environment.signalRUrl + "/text-conversion-hub"
  private hubConnection: HubConnection | undefined;
  private messageSubject$ = new Subject<string>();
  private doneSubject$ = new Subject<void>();
  receivedMessage$ = this.messageSubject$.asObservable();
  done$ = this.doneSubject$.asObservable();

  public joinGroup(groupName: string) {
    return new Promise((resolve, reject) => {
      if (this.hubConnection) {
        this.hubConnection
          .invoke("AddToGroupAsync", groupName)
          .then(() => {
            console.log("Client was added to the group: " + groupName);
            return resolve(true);
          }, (err: any) => {
            return reject(err);
          });
      } else {
        return reject();
      }
    })
  }

  public leaveGroup(groupName: string) {
    return new Promise((resolve, reject) => {
      if (this.hubConnection) {
        this.hubConnection
          .invoke("RemoveFromGroupAsync", groupName)
          .then(() => {
            console.log("Client was removed from the group: " + groupName);
            return resolve(true);
          }, (err: any) => {
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
        .withAutomaticReconnect()
        .build();

      this.hubConnection.start()
        .then(() => {
          console.log("Connection established");
          this.listen();

          return resolve(true);
        })
        .catch((err: any) => {
          reject(err);
        });
    });
  }

  private listen() {
    if (this.hubConnection) {
      (this.hubConnection).on("ConversionResult", (data: string) => {
        this.messageSubject$.next(data);
      });

      (this.hubConnection).on("ConversionDone", () => {
        this.doneSubject$.next();
      });
    }
  }
}