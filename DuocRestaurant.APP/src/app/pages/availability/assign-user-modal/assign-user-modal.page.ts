import { Component, OnInit } from '@angular/core';
import { LoadingController, ToastController, ModalController, AlertController } from '@ionic/angular';
import { TableService } from '@services/table.service';
import { Table } from '@domain/table';
import { User } from '@domain/user';
import { UserService } from '@services/user.service';
import { Profiles } from '@domain/enums';

@Component({
    selector: 'app-assign-user-modal',
    templateUrl: './assign-user-modal.page.html',
    styleUrls: ['./assign-user-modal.page.scss'],
})
export class AssignUserModalPage implements OnInit {

    table: Table;
    users: User[] = [];
    filteredUsers: User[] = [];
    userName: string = null;
    profiles = Profiles;

    constructor(private loadingController: LoadingController,
        private alertController: AlertController,
        private userService: UserService,
        private modalController: ModalController,
        private toastController: ToastController,
        private tableService: TableService) { }

    ngOnInit() {
        this.getUsers(null);
    }

    async getUsers(event) {
        let loading = await this.loadingController.create({
            message: `Obteniendo usuarios`
        });
        await loading.present();

        let filters = {
            RoleId: [this.profiles.Customer, this.profiles.Table]
        };

        this.userService.filterBy(filters).subscribe((users: User[]) => {
            this.users = users;
            this.filteredUsers = users;
            if (event)
                event.target.complete();
            loading.dismiss();
        }, (error) => {
            loading.dismiss();
            if (event)
                event.target.complete();
        });
    }

    async filterUsers(evt) {
        this.filteredUsers = this.users;
        const searchTerm = evt.srcElement.value;

        if (!searchTerm) {
            return;
        }

        this.filteredUsers = this.filteredUsers.filter(x => {
            if (x.Name && searchTerm) {
                return (x.Name.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1);
            }
        });
    }

    async updateAvailability(user: User) {
        const alert = await this.alertController.create({
            header: 'Atención!',
            message: `¿Está seguro de asignar el usuario ${user.Name} ${user.LastName} a la mesa ${this.table.Number}?`,
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
                            message: `Actualizando mesa ${this.table.Number}`
                        });
                        await loading.present();

                        this.table.InUse = !this.table.InUse;
                        this.table.UserId = user.Id;
                        this.tableService.put(this.table.Id, this.table).subscribe((editedTable: Table) => {
                            loading.dismiss();
                            this.modalController.dismiss(editedTable);
                        }, async (error) => {
                            this.table.InUse = !this.table.InUse;
                            this.table.UserId = null;
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
                    }
                }
            ]
        });

        await alert.present();
    }

    dismiss() {
        this.modalController.dismiss();
    }
}
