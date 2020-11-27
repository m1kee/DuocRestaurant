import { Component, OnInit } from '@angular/core';
import { OrderService } from '@services/order.service';
import { LoadingController, ModalController, ToastController } from '@ionic/angular';
import { Order } from '@domain/order';
import { Purchase } from '@domain/purchase';
import { MailService } from '@services/mail.service';
import { PurchaseState } from '@domain/enums';

@Component({
    selector: 'app-details',
    templateUrl: './purchase-detail.page.html',
    styleUrls: ['./purchase-detail.page.scss'],
})
export class PurchaseDetailPage implements OnInit {
    purchase: Purchase;
    orders: Order[] = [];
    purchaseStates = PurchaseState;

    constructor(private orderService: OrderService,
        private loadingController: LoadingController,
        private modalController: ModalController,
        private toastController: ToastController,
        private mailService: MailService
    ) { }

    ngOnInit() {
        this.getOrders();
    }

    sendPayEmail = async () => {
        let loading = await this.loadingController.create({
            message: `Enviando correo`
        });
        await loading.present();

        let order = this.orders[0];

        let mailRequest = {
            MailTo: order.User.Email,
            Subject: 'Link de pago de cuenta',
            Message: `
                <p>Estimado <b>${order.User.Name}</b>:<p>
                <br />
                <p>Puede realizar el pago de su cuenta en el siguiente link: <a href="${this.purchase.URL}?token=${this.purchase.Token}">${this.purchase.URL}?token=${this.purchase.Token}</a></p>
                <br />
                <p>Se despide atentamente <b>Restaurant Siglo XXI</b>.</p>
            `
        };

        this.mailService.sendMail(mailRequest).subscribe(async (response: any) => {
            const toast = await this.toastController.create({
                message: 'Correo de pago enviado',
                position: 'top',
                color: 'success',
                duration: 2000
            });
            toast.present();
            loading.dismiss();
        }, async (error) => {
            const toast = await this.toastController.create({
                message: 'Ocurrió un error enviando el correo',
                position: 'top',
                color: 'danger',
                duration: 2000
            });
            toast.present();
            loading.dismiss();
        });
    };

    close = async () => {
        this.modalController.dismiss();
    };

    getOrders = async () => {
        let loading = await this.loadingController.create({
            message: `Cargando detalle de compra`
        });
        await loading.present();

        let filters = {
            PurchaseId: this.purchase.Id
        };
        this.orderService.filterBy(filters).subscribe((orders: Order[]) => {
            this.orders = orders;
            loading.dismiss();
        }, async (error) => {
            this.orders = [];
            loading.dismiss();

            const toast = await this.toastController.create({
                message: 'Ocurrió un error obteniendo el detalle de la compra',
                position: 'top',
                color: 'danger',
                duration: 2000
            });
            toast.present();
        });
    }

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
