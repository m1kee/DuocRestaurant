import { Component, OnInit } from '@angular/core';
import { Table } from '@app/domain/table';
import { TableService } from '@app/services/table.service';
import { AlertController, LoadingController, ModalController, ToastController } from '@ionic/angular';

@Component({
  selector: 'app-without-booking',
  templateUrl: './without-booking.page.html',
  styleUrls: ['./without-booking.page.scss'],
})
export class WithoutBookingPage implements OnInit {
  diners: number = 1;
  availableTables: Table[] = [];

  constructor(public modalController: ModalController,
    public toastController: ToastController,
    public loadingController: LoadingController,
    public tableService: TableService,
    public alertController: AlertController) { }

  ngOnInit() {
    this.searchAvailableTables(this.diners);
  }

  dismiss() {
    this.modalController.dismiss();
  }

  async searchAvailableTables(diners: number) {
    if (!diners)
      return;

    let loading = await this.loadingController.create({
      message: `Buscando mesas disponibles`
    });
    await loading.present();

    let filters = {
      Capacity: diners,
      Active: true,
      InUse: false
    };

    this.tableService.filterBy(filters).subscribe((tables: Table[]) => {
      this.availableTables = tables;
      loading.dismiss();
    }, async (error) => {
      loading.dismiss();
      let message = 'Ocurrió un error obteniendo las mesas disponibles';

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

  async selectTable(table: Table) {
    const alert = await this.alertController.create({
      header: 'Atención!',
      message: `¿Está seguro que desea ocupar la mesa ${table.Number}?`,
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
              message: `Seleccionando mesa ${table.Number}`
            });
            await loading.present();

            table.InUse = !table.InUse;
            this.tableService.put(table.Id, table).subscribe((editedTable: Table) => {
              this.modalController.dismiss(editedTable);
              loading.dismiss();
            }, async (error) => {
              loading.dismiss();
              let message = 'Ocurri&oacute; un error al seleccionar la mesa.';

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
  }
}
