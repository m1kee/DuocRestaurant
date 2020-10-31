import { Component, OnInit } from '@angular/core';
import { SupplyRequest } from '@domain/supply-request';
import { SupplyRequestService } from '@services/supply-request.service';
import { ModalController, LoadingController, ToastController, AlertController } from '@ionic/angular';
import { AuthService } from '@services/auth.service';
import { User } from '@domain/user';
import { SupplyRequestStates } from '@domain/enums';

@Component({
    selector: 'app-confirm-reception-modal',
    templateUrl: './confirm-reception-modal.page.html',
    styleUrls: ['./confirm-reception-modal.page.scss'],
})
export class ConfirmReceptionModalPage implements OnInit {
    supplyRequestId: number;
    supplyRequest: SupplyRequest = new SupplyRequest();
    loading: boolean = false;
    user: User;

    constructor(private supplyRequestService: SupplyRequestService,
        public modalController: ModalController,
        private authService: AuthService,
        private loadingController: LoadingController,
        private toastController: ToastController,
        private alertController: AlertController
    ) { }

    ngOnInit() {
        this.loading = true;
        this.supplyRequestService.getById(this.supplyRequestId).subscribe((supplyRequest: SupplyRequest) => {
            this.supplyRequest = supplyRequest;
            this.loading = false;
        });
        this.authService.loggedUser.subscribe((user: User) => {
            this.user = user;
        });
    }

    disableConfirmButton() {
        if (!this.supplyRequest)
            return;

        let disable = false;

        if (this.supplyRequest && this.supplyRequest.SupplyRequestDetails) {
            for (let i = 0; i < this.supplyRequest.SupplyRequestDetails.length; i++) {
                let detail = this.supplyRequest.SupplyRequestDetails[i];
                if (!detail.isChecked) {
                    disable = true;
                    break;
                }

            }
        }

        return disable;
    }

    async confirmOrder() {

        let loading = await this.loadingController.create({
            message: `Confirmando recepci&oacute;n`
        });
        await loading.present();

        let finalizeRequest = {
            UserId: this.user.Id,
            SupplyRequestId: this.supplyRequestId,
            SupplyRequestState: SupplyRequestStates.Confirmed
        };

        this.supplyRequestService.finalize(finalizeRequest).subscribe((supplyRequest: SupplyRequest) => {
            loading.dismiss();
            this.modalController.dismiss(supplyRequest);
        }, async (error) => {
            loading.dismiss();
            let message = 'Ocurri&oacute; un error al confirmar la orden de compra';
            if (error.error)
                message = error.error;

            const toast = await this.toastController.create({
                message: message,
                position: 'top',
                color: 'danger',
                duration: 2000
            });
            toast.present();
        });
    }

    async rejectOrder() {
        const alert = await this.alertController.create({
            header: 'Motivo rechazo',
            inputs: [
                {
                    name: 'reason',
                    id: 'reason',
                    type: 'textarea',
                    placeholder: 'Indique el motivo del rechazo'
                }
            ],
            buttons: [
                {
                    text: 'Cancelar',
                    role: 'cancel',
                    cssClass: 'secondary',
                    handler: (blah) => {
                        console.log('Confirm Cancel: blah');
                    }
                }, {
                    text: 'Rechazar',
                    handler: async (data) => {
                        if (!data || !data.reason) {
                            const toast = await this.toastController.create({
                                message: 'Debe indicar la motivo del rechazo.',
                                position: 'top',
                                color: 'danger',
                                duration: 2000
                            });
                            toast.present();
                            return;
                        }

                        let loading = await this.loadingController.create({
                            message: `Rechazando pedido`
                        });
                        await loading.present();

                        let finalizeRequest = {
                            UserId: this.user.Id,
                            Reason: data.reason,
                            SupplyRequestId: this.supplyRequestId,
                            SupplyRequestState: SupplyRequestStates.Rejected
                        };

                        this.supplyRequestService.finalize(finalizeRequest).subscribe((supplyRequest: SupplyRequest) => {
                            loading.dismiss();
                            this.modalController.dismiss(supplyRequest);
                        }, async (error) => {
                            loading.dismiss();
                            let message = 'Ocurri&oacute; un error al rechazar la orden de compra';
                            if (error.error)
                                message = error.error;

                            const toast = await this.toastController.create({
                                message: message,
                                position: 'top',
                                color: 'danger',
                                duration: 2000
                            });
                            toast.present();
                        });
                    }
                }
            ]
        });

        await alert.present();
    }

    dismiss() {
        this.modalController.dismiss();
    }
}
