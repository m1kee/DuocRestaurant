import { Component, OnInit } from '@angular/core';
import { LoadingController, ModalController } from '@ionic/angular';
import { SupplyRequestService } from '@app/services/supply-request.service';
import { SupplyRequest } from '@app/domain/supply-request';
import { SupplyRequestStates } from '@app/domain/enums';
import { ConfirmReceptionModalPage } from './confirm-reception-modal/confirm-reception-modal.page';

@Component({
    selector: 'app-supply-request-reception',
    templateUrl: './supply-request-reception.page.html',
    styleUrls: ['./supply-request-reception.page.scss'],
})
export class SupplyRequestReceptionPage implements OnInit {

    supplyRequestCode: string = null;
    pendingSupplyRequests: SupplyRequest[] = [];
    filteredSupplyRequests: SupplyRequest[] = [];

    constructor(public loadingController: LoadingController,
        public supplyRequestService: SupplyRequestService,
        private modalController: ModalController
    ) { }

    ngOnInit() {
        this.getPendingSupplyRequests(null);
    }

    async getPendingSupplyRequests(event) {
        let loading = await this.loadingController.create({
            message: `Cargando órdenes de compra`
        });
        await loading.present();

        let filters = {
            StateId: SupplyRequestStates.Sended
        };
        this.supplyRequestService.filterBy(filters).subscribe((supplyRequests: SupplyRequest[]) => {
            this.pendingSupplyRequests = supplyRequests;
            this.filteredSupplyRequests = supplyRequests;
            loading.dismiss();
            if (event)
                event.target.complete();
        }, (error) => {
            loading.dismiss();
            if (event)
                event.target.complete();
        });
    }

    async searchSupplyRequest() {
        if (!this.supplyRequestCode)
            return;

        let loading = await this.loadingController.create({
            message: `Cargando orden de compra ${this.supplyRequestCode}`
        });
        await loading.present();

        let filters = {
            StateId: SupplyRequestStates.Sended,
            Code: this.supplyRequestCode
        };
        this.supplyRequestService.filterBy(filters).subscribe((supplyRequests: SupplyRequest[]) => {
            this.pendingSupplyRequests = supplyRequests;
            loading.dismiss();
        }, (error) => {
            loading.dismiss();

        });

    }

    async filterSupplyRequests(evt) {
        this.filteredSupplyRequests = this.pendingSupplyRequests;
        const searchTerm = evt.srcElement.value;

        if (!searchTerm) {
            return;
        }

        this.filteredSupplyRequests = this.filteredSupplyRequests.filter(x => {
            if (x.Code && searchTerm) {
                return (x.Code.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1);
            }
        });
    }

    async showSupplyRequest(supplyRequest: SupplyRequest) {
        const modal = await this.modalController.create({
            component: ConfirmReceptionModalPage,
            componentProps: {
                supplyRequestId: supplyRequest.Id
            }
        });

        modal.onWillDismiss().then((value) => {
            if (value.data) {
                let relatedSupplyRequest = this.pendingSupplyRequests.filter(psr => psr.Id === value.data.Id)[0];
                let index = this.pendingSupplyRequests.indexOf(relatedSupplyRequest);
                this.pendingSupplyRequests.splice(index, 1);
            }
        });

        return await modal.present();
    }
}
