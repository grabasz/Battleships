import { InsertionService } from "./insertion.service";
import { GameService } from "./game.service";
import { BoardService } from "./board.service";
import { Component } from "@angular/core";

@Component({
  selector: "app-root",
  templateUrl: "./components/app.component.html",
  styleUrls: ["./components/app.component.css"],
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
