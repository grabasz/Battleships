import { StatusEnum } from "./status-enum.enum";

export class Tile {
  row: number;
  column: number;
  wasDiscovered: boolean;
  value: StatusEnum;
  isInsertionPreview: boolean;
}
