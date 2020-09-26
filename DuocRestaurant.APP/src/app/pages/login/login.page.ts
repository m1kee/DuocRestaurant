import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Credentials } from '@app/domain/credentials';
import { User } from '@app/domain/user';
import { AuthService } from '@app/services/auth.service';
import { ToastController } from '@ionic/angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {
  credentials: Credentials = {
    Username: null,
    Password: null
  };
  
  loading: boolean = false;

  constructor(private authService: AuthService, 
    private router: Router,
    private toastController: ToastController
  ) { }

  ngOnInit() {
  }

  public signIn = () => {
    console.log('credentials: ', this.credentials);
    this.loading = true;
    this.authService.login(this.credentials).subscribe((user: User) => {
      this.loading = false;
      this.router.navigate(['/home']);
    }, async (response) => {
      this.loading = false;
      let message = '';
      if (response.error)
        message = response.error;
      else
        message = 'Ocurrió un error inesperado, intente nuevamente.';

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
