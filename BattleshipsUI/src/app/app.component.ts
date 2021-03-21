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
export class AppComponent {
  title = "BattleshipsUI";
  success = false;


  isVerticalShip = true;

  constructor(
    private _game: GameService,
    private _insertionService: InsertionService,
    private _boardService: BoardService,

  ) {}

  onResetClick() {
    this._boardService.resetMyBoard();
    this._insertionService.resetInsertion();
  }

  onConfirmClick() {
    this._insertionService.sendShips();
  }
}
