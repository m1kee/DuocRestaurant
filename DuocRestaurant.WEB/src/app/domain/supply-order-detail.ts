import { Product } from './product';

export class SupplyOrderDetail {
    SupplyOrderId: number;
    ProductId: number;
    Count: number;
    Active: boolean;

    Product: Product;

    constructor() {
        this.SupplyOrderId = null;
        this.ProductId = null;
        this.Active = true;
    }
}
