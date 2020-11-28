import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@app/services/auth.service';
import { AlertController } from '@ionic/angular';
import { User } from '@domain/user';

@Component({
    selector: 'app-account',
    templateUrl: './account.page.html',
    styleUrls: ['./account.page.scss'],
})
export class AccountPage implements OnInit {
    user: User;
    constructor(private auth: AuthService,
        private router: Router,
        private alertController: AlertController
    ) { }

    ngOnInit() {
        this.auth.loggedUser.subscribe((user: User) => {
            this.user = user;
        });
    }

    logout = async () => {
        const alert = await this.alertController.create({
            header: 'Atención!',
            message: `¿Está seguro que desea cerrar sesión?`,
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
                        this.auth.logout();
                        this.router.navigate(['/login']);
                    }
                }
            ]
        });

        await alert.present();
    }
}
