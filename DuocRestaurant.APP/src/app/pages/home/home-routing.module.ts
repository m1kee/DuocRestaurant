import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@app/helpers/auth-guard/auth.guard';

import { HomePage } from './home.page';

const routes: Routes = [
    {
        path: '',
        component: HomePage,
        canActivate: [AuthGuard],
        children: [
            {
                path: 'orders',
                loadChildren: () => import('../orders/orders.module').then(m => m.OrdersPageModule)
            },
            {
                path: 'reception',
                loadChildren: () => import('../reception/reception.module').then(m => m.ReceptionPageModule)
            },
            {
                path: 'account',
                loadChildren: () => import('../account/account.module').then(m => m.AccountPageModule)
            },
            {
                path: 'bookings',
                loadChildren: () => import('../bookings/bookings.module').then(m => m.BookingsPageModule)
            },

            {
                path: 'availability',
                loadChildren: () => import('../availability/availability.module').then(m => m.AvailabilityPageModule)
            },
            {
                path: '**',
                loadChildren: () => import('../not-found/not-found.module').then(m => m.NotFoundPageModule)
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HomePageRoutingModule { }
