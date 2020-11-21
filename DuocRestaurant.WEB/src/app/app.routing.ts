import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@app/helpers/auth-guard/auth.guard';
import { LoginComponent } from '@components/shared/login/login.component';
import { PageNotFoundComponent } from '@components/shared/page-not-found/page-not-found.component';
import { HomeComponent } from '@components/home/home.component';
import { TablesComponent } from "@components/maintenance/tables/tables.component";
import { UsersComponent } from "@components/maintenance/users/users.component";
import { BookingSearchComponent } from '@components/bookings/booking-search/booking-search.component';
import { BookingsComponent } from '@components/bookings/booking/bookings.component';
import { ProductComponent } from '@components/maintenance/product/product.component';
import { ProviderComponent } from '@components/maintenance/provider/provider.component';
import { RecipesComponent } from '@components/maintenance/recipes/recipes.component';
import { SupplyRequestComponent } from './components/supply-request/supply-request.component';
import { OrderBoardComponent } from './components/order-board/order-board.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, canActivate: [AuthGuard] },
  { path: 'product-maintenance', component: ProductComponent, canActivate: [AuthGuard] },
  { path: 'provider-maintenance', component: ProviderComponent, canActivate: [AuthGuard] },
  { path: 'tables-maintenance', component: TablesComponent, canActivate: [AuthGuard] },
  { path: 'users-maintenance', component: UsersComponent, canActivate: [AuthGuard] },
  { path: 'booking', component: BookingsComponent, canActivate: [AuthGuard] },
  { path: 'booking-search', component: BookingSearchComponent, canActivate: [AuthGuard] },
  { path: 'recipe-maintenance', component: RecipesComponent, canActivate: [AuthGuard] },
  { path: 'supply-request', component: SupplyRequestComponent, canActivate: [AuthGuard] },
  { path: 'orders-board', component: OrderBoardComponent, canActivate: [AuthGuard] },
  { path: '**', component: PageNotFoundComponent, canActivate: [AuthGuard] }
];

export const APP_ROUTING = RouterModule.forRoot(routes);
