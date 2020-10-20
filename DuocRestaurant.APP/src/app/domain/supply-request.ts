import { Provider } from './provider';
import { SupplyRequestDetail } from './supply-request-detail';

export class SupplyRequest {
    Id: number;
    ProviderId: number;
    SupplyRequestDetails: SupplyRequestDetail[];
    Provider: Provider;
    Date: Date;

    constructor() {
        this.ProviderId = null;
        this.SupplyRequestDetails = [new SupplyRequestDetail()];
    }
}
