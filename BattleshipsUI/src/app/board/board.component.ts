import { GameService } from "./../game.service";
import { InsertionService } from "./../insertion.service";
import { Component, Input, OnInit } from "@angular/core";
import { StatusEnum } from "../status-enum.enum";
import { Status } from "../status";
import { Board } from "../board";

@Component({
  selector: "app-board",
  templateUrl: "./board.component.html",
  styleUrls: ["./board.component.css"],
})
export class BoardComponent implements OnInit {
  @Input() board: Board;
  StatusEnum: typeof StatusEnum = StatusEnum;
  statusMap = {
    0: "🌊",
    1: "X",
    2: "🔥",
    3: "💀",
    4: "🚢",
  };
  constructor(
    private _game: GameService,
    private _insertionService: InsertionService
  ) {}

  ngOnInit() {}
  onClick(status: Status, rowIndex: number, columnIndex: number) {
    if (this._insertionService.isInsertionMode && this.isInsertionActive()) {
      this._insertionService.insertShip(rowIndex, columnIndex);
    }
    if (this._game.isMyTurn()) {
      status.wasDiscovered = true;
    }
  }
  onMouseEnter(rowIndex: number, columnIndex: number) {
    if (this.isInsertionActive()) {
      this._insertionService.showShip(rowIndex, columnIndex);
    }
  }

  private isInsertionActive() {
    return !this._insertionService.awaitConfirmation;
  }

  getCellValue(status: Status): string {
    if (status.wasDiscovered || status.value == StatusEnum.ship) {
      return this.statusMap[status.value];
    }
    if (status.isInsertionPreview) return "🚢";
    return "🌊";
  }

  isOpacityTurnedOn(board: Board): boolean {
    return (
      !this._insertionService.isInsertionMode &&
      (board.isMyBoard || !this._game.isMyTurn())
    );
  }

  headerNumberToLetter(value: number): string{
    return String.fromCharCode(value + 65);
  }
}
