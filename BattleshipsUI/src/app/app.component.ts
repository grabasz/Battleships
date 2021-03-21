import { SignalRService } from './signal-r.service';
import { InsertionService } from "./insertion.service";
import { StatusEnum } from "./status-enum.enum";
import { GameService } from "./game.service";
import { BoardService } from "./board.service";
import { Component, OnInit } from "@angular/core";
import { Status } from "./status";
import { Board } from "./board";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
})
export class AppComponent implements OnInit {
  title = "BattleshipsUI";
  success = false;
  statusMap = {
    0: "🌊",
    1: "X",
    2: "🔥",
    3: "💀",
    4: "🚢",
  };

  isVerticalShip = true;

  constructor(
    private _game: GameService,
    private _insertionService: InsertionService,
    private _boardService: BoardService,
    private _signalRService: SignalRService
  ) {}
  StatusEnum: typeof StatusEnum = StatusEnum;

  ngOnInit() {
    this._signalRService.startConnection();
    this._signalRService.isOpponentReadyListener();

  }

  onClick(status: Status, rowIndex: number, columnIndex: number) {
    if (this._insertionService.isInsertionMode && this.isInsertionActive()) {
      this._insertionService.insertShip(rowIndex, columnIndex);
    }
    if (this._game.isMyTurn()) {
      status.wasDiscovered = true;
    }
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
    if (this.isInsertionActive()) {
      this._insertionService.showShip(rowIndex, columnIndex);
    }
  }

  private isInsertionActive() {
    return !this._insertionService.awaitConfirmation;
  }

  onResetClick() {
    this._boardService.resetMyBoard();
    this._insertionService.resetInsertion();
  }

  onConfirmClick() {
    this._signalRService.sendData();
    this._insertionService.sendShips();
  }
}
