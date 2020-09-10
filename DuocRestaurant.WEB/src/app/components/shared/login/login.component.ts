import { Component, OnInit } from '@angular/core';
import { AuthService } from '@app/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Credentials } from '@domain/credentials';
import { User } from '@domain/user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  credentials: Credentials = {
    Username: null,
    Password: null
  };

  loading: boolean = false;

  constructor(private authService: AuthService, private toastrService: ToastrService, private router: Router) { }

  ngOnInit() {
  }

  public signIn = () => {
    this.loading = true;
    this.authService.login(this.credentials).subscribe((user: User) => {
      this.loading = false;
      this.router.navigate(['/home']);
    }, (response) => {
      this.loading = false;
      let message = '';
      if (response.error)
        message = response.error;
      else
        message = 'Ocurrió un error inesperado, intente nuevamente.';

      this.toastrService.error(message);
    });
  };

  public tryLogin = (event) => {
    if (event.key === "Enter") {
      this.signIn();
    }
  };
}
