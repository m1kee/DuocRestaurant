import { Component, OnInit } from '@angular/core';
import { User } from '@domain/user';
import { AuthService } from '@services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  user: User = null;

  constructor(
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.authService.loggedUser.subscribe((user: User) => {
      this.user = user;
    });
  }

}
