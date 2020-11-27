import { Order } from './order';

export class Purchase {
  Id: number;
  Total: number;
  CreationDate: Date;
  StateId: number;
  URL?: string;
  Token?: string;
  FlowOrder?: number;

  Orders: Order[];

  public constructor() {
    this.Id = 0;
    this.Orders = [];
  }
}
