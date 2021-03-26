import { Component, OnInit } from '@angular/core';
import { BoardService } from 'src/app/services/board.service';
import { InsertionService } from 'src/app/services/insertion.service';

@Component({
  selector: 'app-insertion-options',
  templateUrl: './insertion-options.component.html',
  styleUrls: ['./insertion-options.component.css']
})
export class InsertionOptionsComponent implements OnInit {

  constructor(public _insertionService: InsertionService, private _boardService: BoardService) { }

  ngOnInit() {
  }

  onResetClick() {
    this._boardService.resetMyBoard();
    this._insertionService.resetInsertion();
  }

  onConfirmClick() {
    this._insertionService.sendShips();
  }

}
