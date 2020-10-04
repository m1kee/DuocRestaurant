import { Component, OnInit } from '@angular/core';
import { User } from '@app/domain/user';
import { AuthService } from '@app/services/auth.service';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.page.html',
  styleUrls: ['./welcome.page.scss'],
})
export class WelcomePage implements OnInit {
  user: User = null;
  
  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.loggedUser.subscribe((user: User) => {
      this.user = user;
    });
  }

}
