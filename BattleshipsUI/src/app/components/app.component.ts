import { Component } from "@angular/core";
import { BoardService } from "../services/board.service";
import { GameService } from "../services/game.service";
import { InsertionService } from "../services/insertion.service";

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


}
