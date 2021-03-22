import { BoardService } from './board.service';
import { Injectable } from '@angular/core';
import { ShipsProviderService } from './ships-provider.service';
import { SignalRService } from './signal-r.service';
import { Tile } from '../model/tile';
import { StatusEnum } from '../model/status-enum.enum';

@Injectable({
  providedIn: 'root',
})
export class InsertionService {
  awaitConfirmation: boolean;
  isVertical = true;
  shipSize: number;
  islastValueEmitted: boolean;
  isInsertionMode = true;
  insertedShips: string[][];

  constructor(
    private _boardService: BoardService,
    private shipProvider: ShipsProviderService,
    private _signalRService: SignalRService
  ) {
    this.resetInsertion();
    _signalRService
      .getConnection()
      .on('gameReadyRequest', () => (this.isInsertionMode = false));
  }

  resetInsertion() {
    this.insertedShips = [];
    this.awaitConfirmation = false;
    this.islastValueEmitted = false;
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
        (x) => (x.isInsertionPreview = true)
      );
    } else {
      this.getHorizontalShip(selectedRowIndex, selectedColumnIndex).forEach(
        (x) => (x.isInsertionPreview = true)
      );
    }
  }

  private getVerticalShip(
    selectedRowIndex: number,
    selectedColumnIndex: number
  ) {
    const tiles = this._boardService.myBoard.tiles;
    const verticalShip: Tile[] = [];
    if (selectedRowIndex + this.shipSize > 10) {
      return [];
    }
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

    return verticalShip;
  }

  private getHorizontalShip(
    selectedRowIndex: number,
    selectedColumnIndex: number
  ) {
    const tiles = this._boardService.myBoard.tiles;
    const horizontalShip: Tile[] = [];
    if (selectedColumnIndex + this.shipSize > 10) {
      return [];
    }

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

    return horizontalShip;
  }

  insertShip(selectedRowIndex: number, selectedColumnIndex: number) {
    const fields = this.getFieldsForChange(
      selectedRowIndex,
      selectedColumnIndex
    );
    fields.forEach((x) => (x.value = StatusEnum.ship));
    const lastField = fields[fields.length - 1];
    this.insertedShips.push([
      this.convertToString(selectedRowIndex, selectedColumnIndex),
      this.convertToString(lastField.row, lastField.column),
    ]);
    if (!fields.length) {
      return;
    }

    if (this.islastValueEmitted) {
      this.awaitConfirmation = true;
      const tiles = this._boardService.myBoard.tiles;
      this.resetPreview(tiles);
    }
    this.shipProvider.nextShipSizeTrigger.next();
  }

  private convertToString(row: number, column: number): string {
    const columnNumber = column + 1;
    const rowLetter = String.fromCharCode(row + 65);
    return `${rowLetter}${columnNumber}`;
  }

  private getFieldsForChange(
    selectedRowIndex: number,
    selectedColumnIndex: number
  ): Tile[] {
    if (this.isVertical) {
      return this.getVerticalShip(selectedRowIndex, selectedColumnIndex);
    } else {
      return this.getHorizontalShip(selectedRowIndex, selectedColumnIndex);
    }
  }

  resetPreview(tiles: Tile[][]) {
    tiles.forEach((row) => {
      row.forEach((column) => (column.isInsertionPreview = false));
    });
  }

  sendShips() {
    this._signalRService.sendData(this.insertedShips);
  }
}
