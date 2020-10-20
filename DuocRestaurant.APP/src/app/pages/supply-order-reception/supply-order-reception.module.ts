import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { SupplyOrderReceptionPageRoutingModule } from './supply-order-reception-routing.module';

import { SupplyOrderReceptionPage } from './supply-order-reception.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SupplyOrderReceptionPageRoutingModule
  ],
  declarations: [SupplyOrderReceptionPage]
})
export class SupplyOrderReceptionPageModule {}
