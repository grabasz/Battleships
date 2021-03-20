import { Injectable } from '@angular/core';
import { Board } from './board';

@Injectable({
  providedIn: 'root'
})
export class BoardService {
  myBoard: Board = new Board(true);
  opponentBoard: Board = new Board(false);
  constructor() { }



}
