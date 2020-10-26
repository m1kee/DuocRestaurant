import { Product } from './product';

export class SupplyRequestDetail {
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
