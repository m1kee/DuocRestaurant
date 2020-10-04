import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { BookingsPage } from './bookings.page';

const routes: Routes = [
  {
    path: '',
    component: BookingsPage
  },
  {
    path: 'booking-modal',
    loadChildren: () => import('./booking-modal/booking-modal.module').then( m => m.BookingModalPageModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BookingsPageRoutingModule {}
