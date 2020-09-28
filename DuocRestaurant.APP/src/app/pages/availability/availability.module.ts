import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { AvailabilityPageRoutingModule } from './availability-routing.module';

import { AvailabilityPage } from './availability.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AvailabilityPageRoutingModule
  ],
  declarations: [AvailabilityPage]
})
export class AvailabilityPageModule {}
