import { StatusEnum } from "./status-enum.enum";
import { GameService } from "./game.service";
import { BoardService } from "./board.service";
import { Component } from "@angular/core";
import { Status } from "./status";

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
      // return  === StatusEnum.blank ? "X" : "🔥";
    }
    return "🌊";
  }
}
