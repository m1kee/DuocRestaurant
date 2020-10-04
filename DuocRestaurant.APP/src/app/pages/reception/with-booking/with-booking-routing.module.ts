import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WithBookingPage } from './with-booking.page';

const routes: Routes = [
  {
    path: '',
    component: WithBookingPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WithBookingPageRoutingModule {}
