import { BookingState } from './enums';
import { Table } from './table';
import { User } from './user';

export class Booking {
    Id: number;
    Code: number;
    UserId: number;
    Diners: number;
    TableId: number;
    Date: Date;
    Active: boolean;
    State: BookingState;

    DateFormatted: string;
    TimeFormatted: string;

    User: User;
    Table: Table;

    constructor() {
        this.Active = true;
        this.Diners = 1;
        let date = new Date();
        this.Date = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), 0, 0, 0);
        this.DateFormatted = this.Date.toISOString();
        //this.TimeFormatted = this.Date.toISOString();
    }
  }
  