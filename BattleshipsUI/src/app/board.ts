import { Tile } from "./status";
import { StatusEnum } from "./status-enum.enum";

export class Board {
  id = 1;
  tiles: Tile[][] = new Array<Tile[]>();
  isMyBoard: boolean;

  constructor(isMyBoard: boolean) {
    this.isMyBoard = isMyBoard;

    for (let i = 0; i < 10; i++) {
      const row: Tile[] = new Array<Tile>();
      for (let j = 0; j < 10; j++) {
        row.push({
          row: i,
          column: j,
          value: StatusEnum.blank,
          wasDiscovered: false,
          isInsertionPreview: false,
        });
      }
      this.tiles.push(row);
    }
  }
}
