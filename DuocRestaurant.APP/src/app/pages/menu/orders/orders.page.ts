import { Component, OnInit } from '@angular/core';
import { Order } from '@domain/order';
import { ModalController, AlertController, LoadingController, ToastController } from '@ionic/angular';
import { OrderState } from '@domain/enums';
import { OrderService } from '@services/order.service';
import { PurchaseService } from '@services/purchase.service';
import { Purchase } from '@domain/purchase';
import { User } from '@domain/user';
import { Table } from '@domain/table';

@Component({
    selector: 'app-orders',
    templateUrl: './orders.page.html',
    styleUrls: ['./orders.page.scss'],
})
export class OrdersPage implements OnInit {
    orders: Order[] = [];
    user: User;
    table: Table;
    orderStates = OrderState;

    constructor(
        private modalController: ModalController,
        private orderService: OrderService,
        private purchaseService: PurchaseService,
        private alertController: AlertController,
        private loadingController: LoadingController,
        private toastController: ToastController
    ) { }

    ngOnInit() {
    }

    reloadOrders = (event) => {
        let orderFilter = {
            UserId: this.user.Id,
            States: [this.orderStates.Pending, this.orderStates.InPreparation, this.orderStates.Ready, this.orderStates.Delivered],
            PurchaseId: null
        };

        this.orderService.filterBy(orderFilter).subscribe((orders: Order[]) => {
            this.orders = orders;
            event.target.complete();
        }, () => {
            event.target.complete();
        })
    }

    close = async () => {
        this.modalController.dismiss(this.orders);
    };

    getStateBadgeColor = (order: Order) => {
        let color = '';

        switch (order.StateId) {
            case this.orderStates.Pending:
                color = 'light';
                break;
            case this.orderStates.Canceled:
                color = 'danger';
                break;
            case this.orderStates.InPreparation:
                color = 'warning';
                break;
            case this.orderStates.Ready:
                color = 'success';
                break;
            case this.orderStates.Delivered:
                color = 'dark';
                break;
            default:
                color = 'light';
                break;
        }

        return color;
    };

    getStateBadge = (order: Order) => {
        let state = '';

        switch (order.StateId) {
            case this.orderStates.Pending:
                state = 'Pendiente';
                break;
            case this.orderStates.Canceled:
                state = 'Cancelada';
                break;
            case this.orderStates.InPreparation:
                state = 'En Preparación';
                break;
            case this.orderStates.Ready:
                state = 'Listo';
                break;
            case this.orderStates.Delivered:
                state = 'Entregado';
                break;
            default:
                state = 'Pendiente';
                break;
        }

        return state;
    };

    getOrderTotal = (order: Order) => {
        let total = 0;

        if (order && order.OrderDetails) {
            for (let i = 0; i < order.OrderDetails.length; i++) {
                let detail = order.OrderDetails[i];

                total += detail.Price;
            }
        }

        return total;
    };

    getTotal = () => {
        let total = 0;

        if (this.orders && this.orders.length > 0) {
            for (let i = 0; i < this.orders.length; i++) {
                let order = this.orders[i];

                total += this.getOrderTotal(order);
            }
        }

        return total;
    };

    tryPay = async () => {

        let total = this.getTotal();

        const alert = await this.alertController.create({
            header: 'Pago de cuenta',
            message: `¿Está seguro que desea realizar el pago de su cuenta por un total de $${total}?`,
            buttons: [
                {
                    text: 'No',
                    role: 'cancel',
                    cssClass: 'secondary',
                    handler: () => {
                        // dismiss
                    }
                },
                {
                    text: 'Si',
                    handler: async () => {
                        let loading = await this.loadingController.create({
                            message: `Generando pago`
                        });
                        await loading.present();

                        let purchase: Purchase = new Purchase();
                        purchase.Total = total;
                        purchase.Orders = this.orders;

                        this.purchaseService.post(purchase).subscribe(async (purchase: Purchase) => {
                            loading.dismiss();

                            const toast = await this.toastController.create({
                                message: 'Pago generado, revise su casilla de correo',
                                position: 'top',
                                color: 'success',
                                duration: 2000
                            });
                            toast.present();

                            this.orders = [];
                            this.modalController.dismiss(this.orders);
                        }, async (error) => {
                            loading.dismiss();
                            let message = 'Ocurrió un error al generar el pago.';
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

        await alert.present()
    };

    showPayButton = () => {
        let show = true;

        if (this.orders && this.orders.length > 0) {
            for (let i = 0; i < this.orders.length; i++) {
                let order = this.orders[i];

                if (order.StateId !== this.orderStates.Delivered) {
                    show = false;
                    break;
                }
            }
        }

        return show;
    };

}
