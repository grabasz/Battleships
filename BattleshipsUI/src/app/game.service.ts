import { BoardService } from './board.service';
import { InsertionService } from './insertion.service';
import { Injectable } from "@angular/core";
import { Board } from "./board";

@Injectable({
  providedIn: "root",
})
export class GameService {

  constructor(private _insertionService: InsertionService, private _board: BoardService) {

  }

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
