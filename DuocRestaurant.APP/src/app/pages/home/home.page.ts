import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '@services/auth.service';
import { Profiles } from '@domain/enums';
import * as signalR from '@aspnet/signalr';
import { User } from '@domain/user';
import { environment } from '@env/environment';
import { ToastController } from '@ionic/angular';

@Component({
    selector: 'app-home',
    templateUrl: './home.page.html',
    styleUrls: ['./home.page.scss'],
})
export class HomePage implements OnInit, OnDestroy {
    private env = environment;
    hubConnection: signalR.HubConnection;
    currentUser: User;
    profiles = Profiles;

    constructor(public authService: AuthService,
        private toastController: ToastController
    ) { }

    ngOnInit() {
        this.authService.loggedUser.subscribe((user: User) => {
            this.currentUser = user;

            if (this.currentUser.RoleId === this.profiles.Waiter) {
                this.startHubConnection();
            }
        });
    }

    ngOnDestroy() {
        this.endHubConnection();
    }

    hasPermissions = (requiredPermissions: number[]) => {
        return this.authService.hasPermissions(requiredPermissions);
    };

    startHubConnection = () => {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${this.env.hubUrl}/notifications`)
            .withAutomaticReconnect()
            .build();

        this.hubConnection
            .start()
            .then(() => {
                console.log('Connection started')
                this.hubConnection.on('Notify', async (userId, notification) => {
                    console.log('Notify: ', userId, notification);

                    if (this.currentUser.Id === userId) {
                        const toast = await this.toastController.create({
                            message: notification,
                            position: 'top',
                            color: 'success',
                            duration: 2000
                        });
                        toast.present();
                    }
                })
            })
            .catch(err => {
                console.log('Error while starting connection: ' + err);
                setTimeout(() => {
                    this.startHubConnection();
                }, 5000);
            });

        this.hubConnection.onclose((error) => {
            console.log('Error while starting connection: ' + error);
            setTimeout(() => {
                this.startHubConnection();
            }, 5000);
        });
    };

    endHubConnection = () => {
        this.hubConnection
            .stop()
            .then(() => console.log('Connection closed'))
            .catch((err) => console.log('Error while closing connection: ' + err));
    };
}
