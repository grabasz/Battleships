import { BoardService } from './board.service';
import { Board } from './board';
import { GameService } from "./game.service";
import { Injectable } from "@angular/core";
import { StatusEnum } from "./status-enum.enum";
import { Status } from "./status";
import { ShipsProviderService } from "./ships-provider.service";

@Injectable({
  providedIn: "root",
})
export class InsertionService {
  constructor(
    private _boardService: BoardService,
    private shipProvider: ShipsProviderService
  ) {
    shipProvider.asyncShipSize.subscribe(
      (x) => (this.shipSize = x[0]),
      null,
      () => (this.isInsertionMode = false)
    );
    shipProvider.listener.next();
  }
  isVertical = true;
  shipSize: number;
  isInsertionMode: boolean = true;

  showShip(selectedRowIndex: number, selectedColumnIndex: number) {
    const tiles = this._boardService.myBoard.tiles;
    this.resetPreview(tiles);
    if (this.isVertical) {
      this.insertVerticalShip(selectedRowIndex, selectedColumnIndex).forEach(
        (x) => (x.isInsertedShip = true)
      );
    } else {
      this.getHorizontalShip(selectedRowIndex, selectedColumnIndex).forEach(
        (x) => (x.isInsertedShip = true)
      );
    }
  }

  private insertVerticalShip(
    selectedRowIndex: number,
    selectedColumnIndex: number
  ) {
    const tiles = this._boardService.myBoard.tiles;
    const verticalShip: Status[] = [];
    if (selectedRowIndex + this.shipSize <= 10) {
      for (
        let row = selectedRowIndex;
        row < selectedRowIndex + this.shipSize;
        row++
      ) {
        const status = tiles[row][selectedColumnIndex];
        if (status.value === StatusEnum.ship) {
          return [];
        }
        verticalShip.push(status);
      }
    }
    return verticalShip;
  }

  private getHorizontalShip(
    selectedRowIndex: number,
    selectedColumnIndex: number
  ) {
    const tiles = this._boardService.myBoard.tiles;
    const horizontalShip: Status[] = [];
    if (selectedColumnIndex + this.shipSize <= 10) {
      for (
        let column = selectedColumnIndex;
        column < selectedColumnIndex + this.shipSize;
        column++
      ) {
        const status = tiles[selectedRowIndex][column];
        if (status.value === StatusEnum.ship) {
          return [];
        }
        horizontalShip.push(status);
      }
    }
    return horizontalShip;
  }

  insertShip(selectedRowIndex: number, selectedColumnIndex: number) {
    if (this.isVertical) {
      this.insertVerticalShip(selectedRowIndex, selectedColumnIndex).forEach(
        (x) => (x.value = StatusEnum.ship)
      );
    } else {
      this.getHorizontalShip(selectedRowIndex, selectedColumnIndex).forEach(
        (x) => (x.value = StatusEnum.ship)
      );
    }
    this.shipProvider.listener.next();
  }

  private resetPreview(tiles: Status[][]) {
    tiles.forEach((row) => {
      row.forEach((column) => (column.isInsertedShip = false));
    });
  }
}
