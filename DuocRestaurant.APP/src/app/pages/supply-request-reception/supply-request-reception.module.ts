import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { SupplyRequestReceptionPageRoutingModule } from './supply-request-reception-routing.module';

import { SupplyRequestReceptionPage } from './supply-request-reception.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SupplyRequestReceptionPageRoutingModule
  ],
  declarations: [SupplyRequestReceptionPage]
})
export class SupplyRequestReceptionPageModule {}
