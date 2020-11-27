import { Component, OnInit } from '@angular/core';
import { Order } from '@domain/order';
import { OrderState } from '@domain/enums';
import { OrderService } from '@services/order.service';
import { LoadingController, ToastController } from '@ionic/angular';

@Component({
    selector: 'app-order-list',
    templateUrl: './order-list.page.html',
    styleUrls: ['./order-list.page.scss'],
})
export class OrderListPage implements OnInit {
    orders: Order[] = [];
    orderStates = OrderState;
    constructor(private orderService: OrderService,
        private loadingController: LoadingController,
        private toastController: ToastController
    ) { }

    ngOnInit() {
        this.reloadOrders(null);
    }

    reloadOrders = async (event) => {
        let loading = await this.loadingController.create({
            message: 'Cargando &oacute;rdenes'
        });
        await loading.present();

        let orderFilter = {
            States: [this.orderStates.Pending, this.orderStates.InPreparation, this.orderStates.Ready],
            PurchaseId: null
        };

        this.orderService.filterBy(orderFilter).subscribe((orders: Order[]) => {
            this.orders = orders;

            loading.dismiss();

            if (event)
                event.target.complete();
        }, () => {
            if (event)
                event.target.complete();

            loading.dismiss();
        });
    };

    canDeliver = async (order: Order) => {
        return order.StateId === this.orderStates.Ready;
    }

    deliver = async (order: Order) => {
        let loading = await this.loadingController.create({
            message: 'Entregando orden'
        });
        await loading.present();

        order.StateId = this.orderStates.Delivered;

        this.orderService.put(order.Id, order).subscribe((delivered: Order) => {
            let cIndex = this.orders.findIndex((o) => o.Id === delivered.Id);
            this.orders.splice(cIndex, 1);

            loading.dismiss();
        }, async (error) => {
            order.StateId = this.orderStates.Ready;

            loading.dismiss();

            let message = 'Ocurrió un error al cancelar la reserva.';
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
}
