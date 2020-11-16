import { Component, OnInit } from '@angular/core';
import { Order } from '@domain/order';
import { ModalController } from '@ionic/angular';
import { OrderState } from '@domain/enums';
import { OrderService } from '@services/order.service';

@Component({
    selector: 'app-orders',
    templateUrl: './orders.page.html',
    styleUrls: ['./orders.page.scss'],
})
export class OrdersPage implements OnInit {
    orders: Order[] = [];
    userId: number;
    orderStates = OrderState;

    constructor(
        private modalController: ModalController,
        private orderService: OrderService
    ) { }

    ngOnInit() {
    }

    reloadOrders = (event) => {
        let orderFilter = {
            UserId: this.userId,
            States: [this.orderStates.Pending, this.orderStates.InPreparation, this.orderStates.Ready],
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
        this.modalController.dismiss();
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
