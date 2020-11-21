import { Table } from './table';
import { User } from './user';

export class Purchase {
    Id: number;
    TableId: number;
    UserId: number;
    CreationDate: Date;
    StateId: number;

    Table: Table;
    User: User;

    public constructor() {
        this.Id = 0;
        this.Table = null;
        this.User = null;
    }
}
