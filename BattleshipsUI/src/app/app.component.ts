import { InsertionService } from "./insertion.service";
import { StatusEnum } from "./status-enum.enum";
import { GameService } from "./game.service";
import { BoardService } from "./board.service";
import { Component } from "@angular/core";
import { Status } from "./status";
import { Board } from "./board";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
})
export class AppComponent {
  title = "BattleshipsUI";
  success = false;
  statusMap = {
    0: "🌊",
    1: "X",
    2: "🔥",
    3: "💀",
    4: "🚢",
  };

  isVerticalShip: boolean = true;
  constructor(
    private _game: GameService,
    private _insertionService: InsertionService
  ) {}
  StatusEnum: typeof StatusEnum = StatusEnum;

  onClick(status: Status, rowIndex: number, columnIndex: number) {
    if (this._insertionService.isInsertionMode) {
      this._insertionService.insertShip(rowIndex, columnIndex);
      status.value = StatusEnum.ship;
    }
    if (this._game.isMyTurn()) {
      status.wasDiscovered = true;
    }

    console.log(status);
  }

  getCellValue(status: Status): string {
    if (status.wasDiscovered || status.value == StatusEnum.ship) {
      return this.statusMap[status.value];
    }
    if (status.isInsertedShip) return "🚢";
    return "🌊";
  }

  isOpacityTurnedOn(board: Board) {
    return (
      !this._insertionService.isInsertionMode &&
      (board.isMyBoard || !this._game.isMyTurn())
    );
  }

  onInsertionShipModeChange() {
    // console.log(this.isVerticalShip);
  }

  onMouseEnter(rowIndex: number, columnIndex: number) {
    this._insertionService.showShip(rowIndex, columnIndex);
  }
}
