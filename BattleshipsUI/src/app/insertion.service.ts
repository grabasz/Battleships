import { GameService } from "./game.service";
import { Injectable } from "@angular/core";
import { StatusEnum } from "./status-enum.enum";
import { Board } from "./board";

@Injectable({
  providedIn: "root",
})
export class InsertionService {
  constructor(private _game: GameService) {}
  isVertical = true;
  showShip(rowIndex: number, columnIndex: number) {
    const myBoard = this._game.myBoard;
    this.resetPreview(myBoard);

    myBoard.tiles.forEach((row, rIndex) => {
      row.forEach((column, cIndex) => {
        if (this.isVertical && columnIndex == cIndex) {
          column.isInsertedShip = true;
        }
        if (!this.isVertical && rowIndex == rIndex) {
          column.isInsertedShip = true;
        }
      });
    });
  }

  private resetPreview(myBoard: Board) {
    myBoard.tiles.forEach((row) => {
      row.forEach((column) => (column.isInsertedShip = false));
    });
  }
}
