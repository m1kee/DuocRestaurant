import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { WithBookingPageRoutingModule } from './with-booking-routing.module';

import { WithBookingPage } from './with-booking.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    WithBookingPageRoutingModule
  ],
  declarations: [WithBookingPage]
})
export class WithBookingPageModule {}
