import { Component, OnInit } from '@angular/core';
import { Table } from '@domain/table';
import { TableService } from '@services/table.service';
import { ToastController, LoadingController, ModalController } from '@ionic/angular';
import { AssignUserModalPage } from './assign-user-modal/assign-user-modal.page';

@Component({
    selector: 'app-availability',
    templateUrl: './availability.page.html',
    styleUrls: ['./availability.page.scss'],
})
export class AvailabilityPage implements OnInit {
    tables: Table[] = [];

    constructor(private tableService: TableService,
        private toastController: ToastController,
        private loadingController: LoadingController,
        private modalController: ModalController
    ) {

    }

    ngOnInit() {
        this.getTables(null);
    }

    async getTables(event) {
        let loading = await this.loadingController.create({
            message: 'Obteniendo mesas'
        });
        await loading.present();

        this.tableService.getAll().subscribe((tables: Table[]) => {
            this.tables = tables;
            loading.dismiss();
            if (event)
                event.target.complete();
        }, (error) => {
            loading.dismiss();
            if (event)
                event.target.complete();
        });
    }

    async updateAvailability(table: Table) {
        if (table.InUse) {
            this.freeTable(table);
        }
        else {
            const modal = await this.modalController.create({
                component: AssignUserModalPage,
                componentProps: {
                    table: table
                }
            });

            modal.onWillDismiss().then((editedTable) => {
                if (editedTable.data) {
                    let cIndex = this.tables.findIndex((c) => c.Id === editedTable.data.Id);
                    this.tables.splice(cIndex, 1, editedTable.data);
                }
            });

            return await modal.present();
        }
    }

    async freeTable(table: Table) {
        let loading = await this.loadingController.create({
            message: `Actualizando mesa ${table.Number}`
        });
        await loading.present();

        table.InUse = false;
        table.UserId = null;
        this.tableService.put(table.Id, table).subscribe((editedTable: Table) => {
            let cIndex = this.tables.findIndex((c) => c.Id === editedTable.Id);
            this.tables.splice(cIndex, 1, editedTable);
            loading.dismiss();
        }, async (error) => {
            table.InUse = true;
            table.UserId = table.User.Id;
            loading.dismiss();
            let message = 'Ocurrió un error al actualizar la mesa.';

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
}
