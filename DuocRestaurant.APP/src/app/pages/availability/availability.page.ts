import { Component, OnInit } from '@angular/core';
import { Table } from '@domain/table';
import { TableService } from '@services/table.service';
import { ToastController, LoadingController } from '@ionic/angular';

@Component({
    selector: 'app-availability',
    templateUrl: './availability.page.html',
    styleUrls: ['./availability.page.scss'],
})
export class AvailabilityPage implements OnInit {
    tables: Table[] = [];

    constructor(private tableService: TableService,
        private toastController: ToastController,
        private loadingController: LoadingController) {
        
    }

    ngOnInit() {
        this.getTables();
    }

    async getTables() {
        let loading = await this.loadingController.create({
            message: 'Cargando listado de mesas'
        });
        await loading.present();

        this.tableService.getAll().subscribe((tables: Table[]) => {
            this.tables = tables;
            loading.dismiss();
        }, (error) => {
            loading.dismiss();
        });
    }

    async updateAvailability(table: Table) {
        let loading = await this.loadingController.create({
            message: `Actualizando mesa ${table.Number}`
        });
        await loading.present();

        table.InUse = !table.InUse;
        this.tableService.put(table.Id, table).subscribe((editedTable: Table) => {
            let cIndex = this.tables.findIndex((c) => c.Id === editedTable.Id);
            this.tables.splice(cIndex, 1, editedTable);
            loading.dismiss();
        }, async (error) => {
            loading.dismiss();
            let message = 'Ocurrió un error al actualizar la mesa.';

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
