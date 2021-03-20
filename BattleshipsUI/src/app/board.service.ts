import { ShipsProviderService } from './ships-provider.service';
import { Injectable } from "@angular/core";
import { Board } from "./board";

@Injectable({
  providedIn: "root",
})
export class BoardService {
  myBoard: Board = new Board(true);
  opponentBoard: Board = new Board(false);

  constructor(private _shipProvider: ShipsProviderService) {}

  resetMyBoard() {
    this.myBoard = new Board(true);
    this._shipProvider.reset();
  }
}
