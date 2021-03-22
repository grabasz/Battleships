import { BoardService } from './../board.service';
import { InsertionService } from './../insertion.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-insertion-options',
  templateUrl: './insertion-options.component.html',
  styleUrls: ['./insertion-options.component.css']
})
export class InsertionOptionsComponent implements OnInit {

  constructor(private _insertionService: InsertionService, private _boardService: BoardService) { }

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
