import { Component, OnInit } from '@angular/core';
import { Booking } from '@app/domain/booking';
import { Table } from '@app/domain/table';
import { AlertController, ModalController } from '@ionic/angular';
import { WithBookingPage } from './with-booking/with-booking.page';
import { WithoutBookingPage } from './without-booking/without-booking.page';

@Component({
  selector: 'app-reception',
  templateUrl: './reception.page.html',
  styleUrls: ['./reception.page.scss'],
})
export class ReceptionPage implements OnInit {
  constructor(public modalController: ModalController,
    public alertController: AlertController) { }

  ngOnInit() {
  }

  async withBooking() {
    const modal = await this.modalController.create({
      component: WithBookingPage
    });

    modal.onWillDismiss().then(async (value) => {
      if (value.data) {
        let booking: Booking = value.data;
        const alert = await this.alertController.create({
          header: `Bienvenido ${booking.User.Name} ${booking.User.LastName}`,
          message: `Por favor pase a la mesa ${booking.Table.Number}`
        });

        await alert.present()
      }
    });

    return await modal.present();
  }

  async withoutBooking() {
    const modal = await this.modalController.create({
      component: WithoutBookingPage
    });

    modal.onWillDismiss().then(async (value) => {
      if (value.data) {
        let table: Table = value.data;
        const alert = await this.alertController.create({
          header: `Bienvenido`,
          message: `Por favor pase a la mesa ${table.Number}`
        });

        await alert.present()
      }
    });
    return await modal.present();
  }
}
