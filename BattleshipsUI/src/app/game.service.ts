import { Injectable } from "@angular/core";
import { Board } from "./board";

@Injectable({
  providedIn: "root",
})
export class GameService {
  myBoard: Board = new Board(true);
  opponentBoard: Board = new Board(false);
  constructor() {}

  isAllPlayersReady(): boolean {
    return true;
  }

  isWon(): boolean {
    return false;
  }

  getGameId() {
    return 1;
  }

  getGameUrl() {
    return location.protocol + "//" + location.hostname + this.getPort();
  }

  getPort() {
    return location.port ? ":" + location.port : "";
  }

  GetBords(): Board[] {
    return [this.myBoard, this.opponentBoard];
  }

  isMyTurn() {
    return true;
  }
}
