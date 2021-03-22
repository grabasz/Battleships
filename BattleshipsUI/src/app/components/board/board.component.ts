import { Component, Input, OnInit } from "@angular/core";
import { Board } from "../../model/board";
import { GameService } from "src/app/services/game.service";
import { InsertionService } from "src/app/services/insertion.service";
import { SignalRService } from "src/app/services/signal-r.service";
import { StatusEnum } from "src/app/model/status-enum.enum";
import { Tile } from "src/app/model/tile";

@Component({
  selector: "app-board",
  templateUrl: "./board.component.html",
  styleUrls: ["./board.component.css"],
})
export class BoardComponent implements OnInit {
  @Input() board: Board;
  StatusEnum: typeof StatusEnum = StatusEnum;
  statusMap = {
    0: "🔥",
    1: "X",
    2: "💀",
    3: "🚢",
    4: "🌊",
  };
  constructor(
    private _game: GameService,
    private _insertionService: InsertionService,
    private _signalR: SignalRService
  ) {}

  ngOnInit() {
    if (!this.board.isMyBoard) {
      this.fieldStatusListener("playerFieldStatus");
    } else {
      this.fieldStatusListener("opponentFieldStatus");
    }
  }

  private fieldStatusListener(methodName: string) {
    this._signalR
      .getConnection()
      .on(methodName, (row: number, column: number, status: StatusEnum) => {
        this.board.tiles[row][column].value = status;
        this.board.tiles[row][column].wasDiscovered = true;
      });
  }

  onClick(status: Tile, rowIndex: number, columnIndex: number) {
    if (this._insertionService.isInsertionMode && this.isInsertionActive()) {
      this._insertionService.insertShip(rowIndex, columnIndex);
      return;
    }

    status.wasDiscovered = true;
    this._signalR.play(this._game.getGameId(), rowIndex, columnIndex);
  }

  onMouseEnter(rowIndex: number, columnIndex: number) {
    if (this.isInsertionActive()) {
      this._insertionService.showShip(rowIndex, columnIndex);
    }
  }

  private isInsertionActive() {
    return !this._insertionService.awaitConfirmation;
  }

  getCellValue(status: Tile): string {
    if (status.wasDiscovered || status.value == StatusEnum.ship) {
      return this.statusMap[status.value];
    }
    if (status.isInsertionPreview) return "🚢";
    return "🌊";
  }

  isOpacityTurnedOn(board: Board): boolean {
    return (
      !this._insertionService.isInsertionMode &&
      (board.isMyBoard)
    );
  }

  headerNumberToLetter(value: number): string {
    return String.fromCharCode(value + 65);
  }
}
