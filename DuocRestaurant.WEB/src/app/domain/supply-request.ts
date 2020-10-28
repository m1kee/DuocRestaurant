import { SupplyRequestStates } from './enums';
import { Provider } from './provider';
import { SupplyRequestDetail } from './supply-request-detail';
import { SupplyRequestState } from './supply-request-state';

export class SupplyRequest {
    Id: number;
    Code: string;
    ProviderId: number;
    StateId: number;
    CreationDate: Date;
    
    SupplyRequestDetails: SupplyRequestDetail[];
    Provider: Provider;
    State: SupplyRequestState;
    StateEnum: SupplyRequestStates;

    constructor() {
      this.ProviderId = null;
      this.SupplyRequestDetails = [new SupplyRequestDetail()];
      this.StateEnum = SupplyRequestStates.Created;
    }
}
