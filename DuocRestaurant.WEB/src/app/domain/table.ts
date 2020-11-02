import { User } from './user';

export class Table {
  Id: number;
  Number: number;
  Description: string;
  Capacity: number;
  Active: boolean;
  InUse: boolean;
  UserId?: number;

  User: User;

  public constructor() {
    this.Active = true;
  }
}
