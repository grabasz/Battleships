import { Injectable } from "@angular/core";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

@Injectable({
  providedIn: "root",
})
export class SignalRService {
  constructor() {}

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

  public isOpponentReadyListener() {
    this.hubConnection.on("gameReadyRequest", (user, message) => {
      // this.data = data;
      console.log(user, message);
    });
  }

  public sendData() {
    this.hubConnection
      .invoke("initGame", "test", "aaaaaaa")
      .catch((err) => console.error(err));
  }
}
