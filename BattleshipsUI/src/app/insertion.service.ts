import { BoardService } from './board.service';
import { Injectable } from "@angular/core";
import { StatusEnum } from "./status-enum.enum";
import { Status } from "./status";
import { ShipsProviderService } from "./ships-provider.service";

@Injectable({
  providedIn: "root",
})
export class InsertionService {
  awaitConfirmation: boolean;
  isVertical = true;
  shipSize: number;
  islastValueEmitted: boolean;
  isInsertionMode: boolean = true;

  constructor(
    private _boardService: BoardService,
    private shipProvider: ShipsProviderService
  ) {
    this.subscribeForShipSizeQueue();
  }


  subscribeForShipSizeQueue() {
    this.shipProvider.asyncShipSize.subscribe(
      (x) => (this.shipSize = x[0]),
      null,
      () => (this.islastValueEmitted = true)
    );
    this.shipProvider.nextShipSizeTrigger.next();
  }

  showShip(selectedRowIndex: number, selectedColumnIndex: number) {
    const tiles = this._boardService.myBoard.tiles;
    this.resetPreview(tiles);
    if (this.isVertical) {
      this.getVerticalShip(selectedRowIndex, selectedColumnIndex).forEach(
        (x) => (x.isInsertedShip = true)
      );
    } else {
      this.getHorizontalShip(selectedRowIndex, selectedColumnIndex).forEach(
        (x) => (x.isInsertedShip = true)
      );
    }
  }

  private getVerticalShip(
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
      this.getVerticalShip(selectedRowIndex, selectedColumnIndex).forEach(
        (x) => (x.value = StatusEnum.ship)
      );
    } else {
      this.getHorizontalShip(selectedRowIndex, selectedColumnIndex).forEach(
        (x) => (x.value = StatusEnum.ship)
      );
    }

    if(this.islastValueEmitted) {
    this.awaitConfirmation = true;
    }
    this.shipProvider.nextShipSizeTrigger.next();
  }

  resetPreview(tiles: Status[][]) {
    tiles.forEach((row) => {
      row.forEach((column) => (column.isInsertedShip = false));
    });
  }
}
