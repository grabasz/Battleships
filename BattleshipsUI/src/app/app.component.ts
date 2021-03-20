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
    0: "X",
    1: "🔥",
    2: "💀",
  };

  isVerticalShip: boolean = true;
  constructor(private _board: BoardService, private _game: GameService) {}
  StatusEnum: typeof StatusEnum = StatusEnum;

  fire(status: Status) {
    if (this._game.isMyTurn()) {
      status.wasDiscovered = true;
    }

    console.log(status);
  }

  getCellValue(status: Status): string {
    if (status.wasDiscovered) {
      return this.statusMap[status.value];
    }
    return "🌊";
  }

  isOpacityTurnedOn(board: Board){
    return !this._game.isSetupGameMode() && (board.isMyBoard || !this._game.isMyTurn());
  }

  onInsertionShipModeChange(){
    console.log(this.isVerticalShip);
  }
}
