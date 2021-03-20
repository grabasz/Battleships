import { Status } from "./status";
import { StatusEnum } from "./status-enum.enum";

export class Board {
  id = 1;
  tiles: Status[][] = new Array<Status[]>();
  isMyBoard: boolean;

  constructor(isMyBoard: boolean) {
    this.isMyBoard = isMyBoard;

    for (let i = 0; i < 10; i++) {
      const row: Status[] = new Array<Status>();
      for (let j = 0; j < 10; j++) {
        row.push({
          value: StatusEnum.blank,
          wasDiscovered: false,
          isInsertedShip: false
        });
      }
      this.tiles.push(row);
    }
  }
}
