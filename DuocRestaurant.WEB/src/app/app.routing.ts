import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@app/helpers/auth-guard/auth.guard';
import { LoginComponent } from '@components/shared/login/login.component';
import { PageNotFoundComponent } from '@components/shared/page-not-found/page-not-found.component';
import { HomeComponent } from '@components/home/home.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, canActivate: [AuthGuard] },
  { path: '**', component: PageNotFoundComponent, canActivate: [AuthGuard] }
];

export const APP_ROUTING = RouterModule.forRoot(routes);
