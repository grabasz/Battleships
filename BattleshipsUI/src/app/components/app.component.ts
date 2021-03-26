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
    public _game: GameService,
    public _insertionService: InsertionService,
    public _boardService: BoardService,

  ) {}

  getGame(){
    return this._game;
  }

  getInsertion(){
    return this._insertionService;
  }

  getBoard(){
    return this._boardService;
  }


}
