import { Component, OnInit } from '@angular/core';
import { Order } from '@domain/order';
import { ModalController, LoadingController, ToastController } from '@ionic/angular';
import { OrderDetail } from '@domain/order-detail';
import { PurchaseService } from '@services/purchase.service';
import { OrderService } from '@services/order.service';
import { Purchase } from '../../../domain/purchase';

@Component({
    selector: 'app-cart',
    templateUrl: './cart.page.html',
    styleUrls: ['./cart.page.scss'],
})
export class CartPage implements OnInit {
    order: Order;

    constructor(private modalController: ModalController,
        private loadingController: LoadingController,
        private purchaseService: PurchaseService,
        private orderService: OrderService,
        private toastController: ToastController
    ) { }

    ngOnInit() {
    }

    close = async () => {
        this.modalController.dismiss(this.order);
    };

    add = async (detail: OrderDetail) => {
        detail.Count++;
    };

    subtract = async (detail: OrderDetail) => {
        detail.Count--;
    };

    remove = async (detail: OrderDetail) => {
        let removeIndex = this.order.OrderDetails.findIndex(od => od.ProductId === detail.ProductId && od.RecipeId === detail.RecipeId);
        if (removeIndex > -1) {
            this.order.OrderDetails.splice(removeIndex, 1);
        }
    };

    makeOrder = async () => {
        let loading = await this.loadingController.create({
            message: `Realizando pedido`
        });
        await loading.present();
        let dismissLoading = () => { loading.dismiss(); };


        this.orderService.post(this.order).subscribe(async (order: Order) => {
            dismissLoading();
            this.order = order;

            const toast = await this.toastController.create({
                message: 'Orden creada con éxito',
                position: 'top',
                color: 'success',
                duration: 2000
            });
            toast.present();

            this.modalController.dismiss(this.order);
        }, async () => {
            dismissLoading();

            const toast = await this.toastController.create({
                message: 'Ocurrió un error creando la orden, porfavor intente nuevamente.',
                position: 'top',
                color: 'danger',
                duration: 2000
            });
            toast.present();
        });
    };
}
