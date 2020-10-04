import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReceptionPage } from './reception.page';

const routes: Routes = [
  {
    path: '',
    component: ReceptionPage
  },
  {
    path: 'with-booking',
    loadChildren: () => import('./with-booking/with-booking.module').then( m => m.WithBookingPageModule)
  },
  {
    path: 'without-booking',
    loadChildren: () => import('./without-booking/without-booking.module').then( m => m.WithoutBookingPageModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReceptionPageRoutingModule {}
