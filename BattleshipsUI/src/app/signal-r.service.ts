import { Injectable } from "@angular/core";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

@Injectable({
  providedIn: "root",
})
export class SignalRService {
  constructor() {
    this.startConnection();
  }

  private hubConnection: HubConnection;

  public startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl("https://localhost:44365/hub")
      .build();
    this.hubConnection
      .start()
      .then(() => console.log("Connection started"))
      .catch((err) => console.log("Error while starting connection: " + err));
  }

  public sendData(data: string[][]) {
    this.hubConnection
      .invoke("initGame", data)
      .catch((err) => console.error(err));
  }

  public getConnection():HubConnection{
    return this.hubConnection;
  }
}
