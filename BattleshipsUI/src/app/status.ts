import { StatusEnum } from "./status-enum.enum";

export class Status {
  row: number;
  column: number;
  wasDiscovered: boolean;
  value: StatusEnum;
  isInsertionPreview: boolean;
}
