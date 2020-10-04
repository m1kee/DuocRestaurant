import { Component, OnInit } from '@angular/core';
import { Booking } from '@app/domain/booking';
import { BookingState } from '@app/domain/enums';
import { User } from '@app/domain/user';
import { AuthService } from '@app/services/auth.service';
import { BookingService } from '@app/services/booking.service';
import { AlertController, LoadingController, ModalController, ToastController } from '@ionic/angular';
import { BookingModalPage } from './booking-modal/booking-modal.page';

@Component({
    selector: 'app-bookings',
    templateUrl: './bookings.page.html',
    styleUrls: ['./bookings.page.scss'],
})
export class BookingsPage implements OnInit {
    bookings: Booking[] = [];
    currentUser: User = null;
    bookingStates = BookingState;

    constructor(
        private modalController: ModalController,
        public bookingService: BookingService,
        public authService: AuthService,
        public alertController: AlertController,
        private loadingController: LoadingController,
        private toastController: ToastController
    ) { }

    async ngOnInit() {
        let loading = await this.loadingController.create({
            message: `Obteniendo reservas`
        });
        await loading.present();

        this.authService.loggedUser.subscribe((user: User) => {
            this.currentUser = user;
            let filters = {
                UserId: this.currentUser.Id
            }

            this.bookingService.filterBy(filters).subscribe((bookings: Booking[]) => {
                loading.dismiss();
                this.bookings = bookings.sort((a, b) => this.sortDates(a.Date, b.Date));
                //console.log('my bookings: ', this.bookings);
            }, (error) => { loading.dismiss(); });

        }, (error) => { loading.dismiss(); });
    }

    sortDates(a, b) {
        let thisDate = new Date(b);
        let thatDate = new Date(a);

        return thisDate.getTime() - thatDate.getTime();
    }

    async newBooking() {
        const modal = await this.modalController.create({
            component: BookingModalPage
        });

        modal.onWillDismiss().then((value) => {
            if (value.data) {
                this.bookings.splice(0, 0, value.data);
            }
        });

        return await modal.present();
    }

    cancelableBooking(booking: Booking) {
        let date = new Date();
        let bookingDate = new Date(booking.Date);
        //console.log('cancelable: ', bookingDate > date, bookingDate, date);
        return bookingDate > date;
    };

    async cancelBooking(booking: Booking) {
        const alert = await this.alertController.create({
            header: 'Atención!',
            message: `¿Está seguro que desea cancelar la reserva?`,
            buttons: [
                {
                    text: 'No',
                    role: 'cancel',
                    cssClass: 'secondary',
                    handler: () => {
                        // dismiss
                    }
                }, {
                    text: 'Si',
                    handler: async () => {
                        let loading = await this.loadingController.create({
                            message: `Cancelando reserva`
                        });
                        await loading.present();

                        this.bookingService.delete(booking.Id).subscribe(() => {
                            let cIndex = this.bookings.findIndex((c) => c.Id === booking.Id);
                            this.bookings.splice(cIndex, 1);
                            loading.dismiss();

                        }, async (error) => {
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
                    }
                }
            ]
        });

        await alert.present()
    };
}
