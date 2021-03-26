import { SignalRService } from "./signal-r.service";
import { BoardService } from "./board.service";
import { InsertionService } from "./insertion.service";
import { Injectable } from "@angular/core";
import { Board } from "../model/board";

@Injectable({
  providedIn: "root",
})
export class GameService {
  gameId: number;
  isGameWon: boolean;
  isGameFail: boolean;

  constructor(
    private _insertionService: InsertionService,
    private _board: BoardService,
    private _signalR: SignalRService
  ) {
    this.gameReadyListener();
    this.gameWonListener();
    this.gameFailListener();
  }

  private gameReadyListener() {
    this._signalR
      .getConnection()
      .on("gameReadyRequest", (gameId: number) => (this.gameId = gameId));
  }

  private gameWonListener() {
    this._signalR
      .getConnection()
      .on("gameWon", (gameId: number) => (this.isGameWon = true));
  }
  private gameFailListener() {
    this._signalR
      .getConnection()
      .on("gameFail", (gameId: number) => (this.isGameFail = true));
  }

  public isAllPlayersReady(): boolean {
    return true;
  }

  isWon(): boolean {
    return this.isGameWon;
  }

  isFail() {
    return this.isGameFail;
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

  GetBords(): Board[] {
    if (this.isInsertionGameMode()) {
      return [this._board.myBoard];
    }

    return [this._board.myBoard, this._board.opponentBoard];
  }
  isInsertionGameMode(): boolean {
    return this._insertionService.isInsertionMode;
  }
}
