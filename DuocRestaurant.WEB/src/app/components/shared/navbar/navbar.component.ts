import { Component, OnInit } from '@angular/core';
import { faShoppingCart, faAngleDown, faSignOutAlt, faUserCircle, faList, faBoxOpen, faStore, faBoxes, faCreditCard, faBroom, faFrown, faPlus, faMinus, faTrashAlt, faReceipt } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '@services/auth.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Profiles } from '@domain/enums';
import { Router } from '@angular/router';
import { User } from '@app/domain/user';
import { Credentials } from '@app/domain/credentials';

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

  profiles = Profiles;

  ngOnInit() {
    this.authService.loggedUser.subscribe((user: User) => {
      this.user = user;
    });
  };

  // Authentication stuff
  credentials: Credentials = {
    Username: null,
    Password: null
  };

  user: User = null;

  logout = () => {
    this.authService.logout();
    this.router.navigate(['/login']);
  };

  isAuthenticated = () => {
    return this.authService.isAuthenticated();
  };

  hasPermissions = (requiredPermissions: number[]) => {
    return this.authService.hasPermissions(requiredPermissions);
  }; 
}
