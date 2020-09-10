import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@app/helpers/auth-guard/auth.guard';
import { LoginComponent } from '@components/shared/login/login.component';
import { PageNotFoundComponent } from '@components/shared/page-not-found/page-not-found.component';
import { HomeComponent } from '@components/home/home.component';
import { InventoryComponent } from "@components/maintenance/inventory/inventory.component";
import { TablesComponent } from "@components/maintenance/tables/tables.component";
import { UsersComponent } from "@components/maintenance/users/users.component";
import { BookingSearchComponent } from '@components/bookings/booking-search/booking-search.component';
import { BookingsComponent } from '@components/bookings/booking/bookings.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, canActivate: [AuthGuard] },
  { path: 'inventory-maintenance', component: InventoryComponent, canActivate: [AuthGuard] },
  { path: 'tables-maintenance', component: TablesComponent, canActivate: [AuthGuard] },
  { path: 'users-maintenance', component: UsersComponent, canActivate: [AuthGuard] },
  { path: 'booking', component: BookingsComponent, canActivate: [AuthGuard] },
  { path: 'booking-search', component: BookingSearchComponent, canActivate: [AuthGuard] },
  
  { path: '**', component: PageNotFoundComponent, canActivate: [AuthGuard] }
];

export const APP_ROUTING = RouterModule.forRoot(routes);
