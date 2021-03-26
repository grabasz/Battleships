import { environment } from './../../environments/environment.prod';
// import { environment } from "./../../environments/environment";
import { Injectable } from "@angular/core";
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";

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
      .configureLogging(LogLevel.Debug)
      .withUrl(this.getApiUrl(), this.getOptions())
      .build();
    this.hubConnection
      .start()
      .then(() => console.log("Connection started"))
      .catch((err) => console.log("Error while starting connection: " + err));
  }

  private getOptions() {
    console.log("Setting options for SignalR");
    if (environment.production) {
      console.log("SignalR negotiation disabled");
      return {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      };
    }
  }

  private getApiUrl(): string {
    return environment.apiUrl;
  }

  public sendData(data: string[][]) {
    this.hubConnection
      .invoke("initGame", data)
      .catch((err) => console.error(err));
  }

  public play(gameId: number, row: number, column: number) {
    this.hubConnection
      .invoke("play", gameId, row, column)
      .catch((err) => console.error(err));
  }

  public getConnection(): HubConnection {
    return this.hubConnection;
  }
}
