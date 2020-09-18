export class Table {
    Id: number;
    Number: number;
    Description: string;
    Capacity: number;
    Active: boolean;
    InUse: boolean;

  public constructor() {
    this.Active = true;
  }
}
