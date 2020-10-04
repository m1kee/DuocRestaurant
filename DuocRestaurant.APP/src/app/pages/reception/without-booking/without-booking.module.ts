import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { WithoutBookingPageRoutingModule } from './without-booking-routing.module';

import { WithoutBookingPage } from './without-booking.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    WithoutBookingPageRoutingModule
  ],
  declarations: [WithoutBookingPage]
})
export class WithoutBookingPageModule {}
