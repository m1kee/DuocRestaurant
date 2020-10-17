import { Provider } from './provider';
import { SupplyOrderDetail } from './supply-order-detail';

export class SupplyOrder {
    Id: number;
    ProviderId: number;
    SupplyOrderDetails: SupplyOrderDetail[];
    Provider: Provider;

    constructor() {
        this.ProviderId = null;
        this.SupplyOrderDetails = [new SupplyOrderDetail()];
    }
}
