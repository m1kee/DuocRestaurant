import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { SupplyRequestReceptionPageRoutingModule } from './supply-request-reception-routing.module';

import { SupplyRequestReceptionPage } from './supply-request-reception.page';
import { SupplyCodePipe } from '@pipes/supply-code.pipe';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SupplyRequestReceptionPageRoutingModule
  ],
  declarations: [SupplyRequestReceptionPage, SupplyCodePipe]
})
export class SupplyRequestReceptionPageModule {}
