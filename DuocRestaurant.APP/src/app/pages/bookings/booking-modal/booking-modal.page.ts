import { Component, OnInit } from '@angular/core';
import { Booking } from '@app/domain/booking';
import { Table } from '@app/domain/table';
import { User } from '@app/domain/user';
import { MONTH_NAMES } from '@app/helpers/date/date-helper';
import { AuthService } from '@app/services/auth.service';
import { BookingService } from '@app/services/booking.service';
import { TableService } from '@app/services/table.service';
import { LoadingController, ModalController, ToastController } from '@ionic/angular';

@Component({
  selector: 'app-booking-modal',
  templateUrl: './booking-modal.page.html',
  styleUrls: ['./booking-modal.page.scss'],
})
export class BookingModalPage implements OnInit {
  currentUser: User = null;
  booking: Booking = null;
  numberPattern = "^[0-9]+$";
  maxDate: string;
  minDate: string;
  tables: Table[] = [];
  monthNames: string[] = MONTH_NAMES;
  loading: boolean;

  constructor(
    public modalController: ModalController,
    public authService: AuthService,
    public tableService: TableService,
    public bookingService: BookingService,
    public toastController: ToastController,
    public loadingController: LoadingController
  ) {
    this.booking = new Booking();
    this.minDate = new Date().toISOString();
    let maxDate = new Date();
    maxDate.setFullYear(new Date().getFullYear() + 1);
    this.maxDate = maxDate.toISOString();
  }

  ngOnInit() {
    this.authService.loggedUser.subscribe((user: User) => {
      this.currentUser = user;
      this.booking.UserId = this.currentUser.Id;
    });

    this.getTables();
  }

  dismiss() {
    this.modalController.dismiss();
  }

  getTables() {
    if (!this.booking.Diners)
      return;

    this.booking.TableId = null;

    let filters = {
      Capacity: this.booking.Diners
    };

    this.tableService.filterBy(filters).subscribe((tables: Table[]) => {
      this.tables = tables;
    });
  }

  async save() {
    let loading = await this.loadingController.create({
      message: `Creando reserva`
    });
    await loading.present();

    let date = new Date(this.booking.DateFormatted);
    let time = new Date(this.booking.TimeFormatted);
    this.booking.Date = new Date(date.getFullYear(), date.getMonth(), date.getDate(), time.getHours(), time.getMinutes(), 0, 0);

    this.bookingService.post(this.booking).subscribe(async (createdBooking) => {
      loading.dismiss();
      const toast = await this.toastController.create({
        message: 'Reserva creada con éxito',
        position: 'top',
        color: 'success',
        duration: 2000
      });
      toast.present();
      this.modalController.dismiss(createdBooking);
    }, async (error) => {
      loading.dismiss();
      let message = 'Ocurrió un error al crear la reserva';
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
