import { Product } from './product';

export class SupplyRequestDetail {
    SupplyRequestId: number;
    ProductId: number;
    Count: number;
    Active: boolean;

    // to confirm reception when all details are checked
    isChecked: boolean;

    Product: Product;

    constructor() {
        this.SupplyRequestId = null;
        this.ProductId = null;
        this.Active = true;
    }
}
