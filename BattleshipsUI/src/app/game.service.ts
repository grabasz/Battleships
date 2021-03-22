import { SignalRService } from './signal-r.service';
import { BoardService } from './board.service';
import { InsertionService } from './insertion.service';
import { Injectable } from "@angular/core";
import { Board } from "./board";

@Injectable({
  providedIn: "root",
})
export class GameService {
  gameId: number;
  isGameWon: boolean;

  constructor(private _insertionService: InsertionService, private _board: BoardService, private _signalR: SignalRService) {
    this.gameReadyListener(_signalR);
    this.gameWonListener(_signalR);
  }

  private gameReadyListener(_signalR: SignalRService) {
    _signalR
      .getConnection()
      .on("gameReadyRequest", (gameId: number) => (this.gameId = gameId));
  }

  private gameWonListener(_signalR: SignalRService) {
    _signalR
      .getConnection()
      .on("gameWon", (gameId: number) => (this.isGameWon = true));
  }

  isAllPlayersReady(): boolean {
    return true;
  }

  isWon(): boolean {
    return this.isGameWon;
  }

  getGameId() {
    return this.gameId;
  }

  getGameUrl() {
    return location.protocol + "//" + location.hostname + this.getPort();
  }

  getPort() {
    return location.port ? ":" + location.port : "";
  }

  isMyTurn() {
    return true;
  }

  GetBords(): Board[] {
    if(this.isInsertionGameMode()) {
      return [this._board.myBoard];
    }

    return [this._board.myBoard, this._board.opponentBoard];
  }
  isInsertionGameMode(): boolean{
    return this._insertionService.isInsertionMode;
  }
}
