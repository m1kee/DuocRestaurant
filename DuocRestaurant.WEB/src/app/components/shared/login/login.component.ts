import { Component, OnInit } from '@angular/core';
import { ICredentials } from '@app/domain/credentials';
import { AuthService } from '@app/services/auth.service';
import { IUser } from '@app/domain/user';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  credentials: ICredentials = {
    Username: null,
    Password: null
  };

  loading: boolean = false;

  constructor(private authService: AuthService, private toastrService: ToastrService, private router: Router) { }

  ngOnInit() {
  }

  public signIn = () => {
    this.loading = true;
    this.authService.login(this.credentials).subscribe((user: IUser) => {
      this.loading = false;
      this.router.navigate(['/home']);
    }, (response) => {
      this.loading = false;
      let message = '';
      if (response.error && response.error.message)
        message = response.error.message;
      else
        message = 'Unexpected error, try again.';

      this.toastrService.error(message);
    });
  };

  public tryLogin = (event) => {
    if (event.key === "Enter") {
      this.signIn();
    }
  };
}
