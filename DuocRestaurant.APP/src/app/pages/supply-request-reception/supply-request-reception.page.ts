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

    constructor(public loadingController: LoadingController,
        public supplyRequestService: SupplyRequestService,
        private modalController: ModalController
    ) { }

    ngOnInit() {
        this.getPendingSupplyRequests();
    }

    async getPendingSupplyRequests() {
        let loading = await this.loadingController.create({
            message: `Cargando órdenes de compra`
        });
        await loading.present();

        let filters = {
            StateId: SupplyRequestStates.Sended
        };
        this.supplyRequestService.filterBy(filters).subscribe((supplyRequests: SupplyRequest[]) => {
            this.pendingSupplyRequests = supplyRequests;
            loading.dismiss();
        }, (error) => {
            loading.dismiss();

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
