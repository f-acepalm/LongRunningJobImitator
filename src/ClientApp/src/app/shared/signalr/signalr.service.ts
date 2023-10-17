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
  private canceledSubject$ = new Subject<void>();
  private doneSubject$ = new Subject<void>();
  receivedMessage$ = this.messageSubject$.asObservable();
  canceled$ = this.canceledSubject$.asObservable();
  done$ = this.doneSubject$.asObservable();

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

  public leaveGroup(groupName: string) {
    return new Promise((resolve, reject) => {
      if (this.hubConnection) {
        this.hubConnection
          .invoke("RemoveFromGroupAsync", groupName)
          .then(() => {
            console.log("Removed from the group");
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
        .withAutomaticReconnect()
        .build();

      this.hubConnection.start()
        .then(() => {
          console.log("connection established");
          this.listen();

          return resolve(true);
        })
        .catch((err: any) => {
          console.log("error occured" + err);
          reject(err);
        });
    });
  }

  private listen() {
    if (this.hubConnection) {
      (this.hubConnection).on("ConversionResult", (data: string) => {
        this.messageSubject$.next(data);
      });

      (this.hubConnection).on("ConversionCanceled", () => {
        console.log('Canceled');
        this.canceledSubject$.next();
      });

      (this.hubConnection).on("ConversionDone", () => {
        console.log('Done');
        this.doneSubject$.next();
      });
    }
  }
}