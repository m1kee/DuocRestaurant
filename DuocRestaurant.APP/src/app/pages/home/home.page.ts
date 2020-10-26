import { Component, OnInit } from '@angular/core';
import { AuthService } from '@services/auth.service';
import { Profiles } from '@domain/enums';

@Component({
    selector: 'app-home',
    templateUrl: './home.page.html',
    styleUrls: ['./home.page.scss'],
})
export class HomePage implements OnInit {

    constructor(public authService: AuthService) { }

    ngOnInit() {
    }

    profiles = Profiles;

    hasPermissions = (requiredPermissions: number[]) => {
        return this.authService.hasPermissions(requiredPermissions);
    };

}
