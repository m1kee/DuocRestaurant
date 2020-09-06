import { Component, OnInit } from '@angular/core';
import { faShoppingCart, faAngleDown, faSignOutAlt, faUserCircle, faList, faBoxOpen, faStore, faBoxes, faCreditCard, faBroom, faFrown, faPlus, faMinus, faTrashAlt, faReceipt } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '@services/auth.service';
import { ICredentials } from '@domain/credentials';
import { IUser } from '@domain/user';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Profiles } from '@domain/enums';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private toastrService: ToastrService,
    private router: Router
  ) { }

  categories: any = [];
  icons: any = {
    faShoppingCart: faShoppingCart,
    faAngleDown: faAngleDown,
    faSignOutAlt: faSignOutAlt,
    faUserCircle: faUserCircle,
  };
  menuBehavior: any = {
    isCollapsed: true
  };

  admin: Profiles = Profiles.Administrator;

  ngOnInit() {
    this.authService.loggedUser.subscribe((user: IUser) => {
      this.user = user;
    });
  };

  // Authentication stuff
  credentials: ICredentials = {
    Username: null,
    Password: null
  };
  user: IUser = null;

  logout = () => {
    this.authService.logout();
    this.router.navigate(['']);
  };

  isAuthenticated = () => {
    return this.authService.isAuthenticated();
  };
}
