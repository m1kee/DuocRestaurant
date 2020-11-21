import { OrderState } from './enums';
import { Purchase } from './purchase';
import { OrderDetail } from './order-detail';
import { Table } from './table';
import { User } from './user';

export class Order {
  Id: number;
  PurchaseId?: number;
  UserId: number;
  TableId: number;
  StateId: number;
  Note: string;

  _showDetails: boolean;
  _isLoading: boolean;

  User: User;
  Table: Table;
  OrderState: OrderState;
  Purchase: Purchase;
  OrderDetails: OrderDetail[];

  public constructor() {
    this.Purchase = null;
    this.User = null;
    this.Table = null;
    this.OrderDetails = [];
  }
}
