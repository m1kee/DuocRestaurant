import { Product } from './product';

export class SupplyRequestDetail {
    SupplyRequestId: number;
    ProductId: number;
    Count: number;
    Active: boolean;

    Product: Product;

    constructor() {
        this.SupplyRequestId = null;
        this.ProductId = null;
        this.Active = true;
    }
}
