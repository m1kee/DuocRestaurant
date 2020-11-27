import { Component, OnInit } from '@angular/core';
import { PurchaseService } from '@services/purchase.service';
import { LoadingController, AlertController, ModalController, ToastController } from '@ionic/angular';
import { Purchase } from '../../domain/purchase';
import { AuthService } from '../../services/auth.service';
import { User } from '../../domain/user';
import { PurchaseState } from '../../domain/enums';
import { PurchaseDetailPage } from './purchase-detail/purchase-detail.page';

@Component({
    selector: 'app-my-purchases',
    templateUrl: './my-purchases.page.html',
    styleUrls: ['./my-purchases.page.scss'],
})
export class MyPurchasesPage implements OnInit {
    purchases: Purchase[] = [];
    currentUser: User;
    purchaseStates = PurchaseState;

    constructor(private purchaseService: PurchaseService,
        private authService: AuthService,
        private loadingController: LoadingController,
        private modalController: ModalController,
        private toastController: ToastController
    ) { }

    ngOnInit() {
        this.refreshPayments(null);
    }

    refreshPayments = async ($event) => {
        let loading = await this.loadingController.create({
            message: `Cargando mis compras`
        });
        await loading.present();

        this.authService.loggedUser.subscribe((user: User) => {
            this.currentUser = user;
            let filters = {
                UserId: this.currentUser.Id
            };

            this.purchaseService.filterBy(filters).subscribe((purchases: Purchase[]) => {
                loading.dismiss();
                if (purchases && purchases[0]) {
                    this.purchases = purchases;
                }
                else {
                    this.purchases = null;
                }

                if ($event)
                    $event.target.complete();
            }, async () => {
                this.purchases = null;
                loading.dismiss();
                if ($event)
                    $event.target.complete();
                const toast = await this.toastController.create({
                    message: 'Ocurrió un error obteniendo tus compras',
                    position: 'top',
                    color: 'danger',
                    duration: 2000
                });
                toast.present();
            });
        });
    };
    viewDetails = async (purchase: Purchase) => {
        const modal = await this.modalController.create({
            component: PurchaseDetailPage,
            componentProps: {
                purchase: purchase
            }
        });

        return await modal.present();
    };
    getStateBadgeColor = (purchase: Purchase) => {
        let color = '';

        switch (purchase.StateId) {
            case this.purchaseStates.PendingPayment:
                color = 'light';
                break;
            case this.purchaseStates.Rejected:
                color = 'danger';
                break;
            case this.purchaseStates.Canceled:
                color = 'warning';
                break;
            case this.purchaseStates.Paid:
                color = 'success';
                break;
            default:
                color = 'light';
                break;
        }

        return color;
    };
    getStateBadge = (purchase: Purchase) => {
        let state = '';

        switch (purchase.StateId) {
            case this.purchaseStates.PendingPayment:
                state = 'Pendiente';
                break;
            case this.purchaseStates.Canceled:
                state = 'Cancelada';
                break;
            case this.purchaseStates.Rejected:
                state = 'Rechazada';
                break;
            case this.purchaseStates.Paid:
                state = 'Pagada';
                break;
            default:
                state = 'Pendiente';
                break;
        }

        return state;
    };
}
