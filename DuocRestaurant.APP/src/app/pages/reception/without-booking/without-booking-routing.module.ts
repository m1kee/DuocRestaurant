import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WithoutBookingPage } from './without-booking.page';

const routes: Routes = [
  {
    path: '',
    component: WithoutBookingPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WithoutBookingPageRoutingModule {}
