import { Component, OnInit } from '@angular/core';
import { Booking } from '@app/domain/booking';
import { Table } from '@app/domain/table';
import { BookingService } from '@app/services/booking.service';
import { TableService } from '@app/services/table.service';
import { LoadingController, ModalController, ToastController } from '@ionic/angular';

@Component({
    selector: 'app-with-booking',
    templateUrl: './with-booking.page.html',
    styleUrls: ['./with-booking.page.scss'],
})
export class WithBookingPage implements OnInit {
    code: number = null;

    constructor(public modalController: ModalController,
        public bookingService: BookingService,
        public toastController: ToastController,
        public loadingController: LoadingController,
        public tableService: TableService) { }

    ngOnInit() {
    }

    dismiss() {
        this.modalController.dismiss();
    }

    async searchBooking(code: number) {
        if (!code)
            return;

        let loading = await this.loadingController.create({
            message: `Buscando reserva`
        });
        await loading.present();

        this.bookingService.getByCode(code).subscribe((booking: Booking) => {
            booking.Table.InUse = !booking.Table.InUse;
            this.tableService.put(booking.Table.Id, booking.Table).subscribe((editedTable: Table) => {
                loading.dismiss();
                this.modalController.dismiss(booking);
            });
        }, async (error) => {
            loading.dismiss();
            let message = 'Ocurrió un error al buscar la reserva';
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
