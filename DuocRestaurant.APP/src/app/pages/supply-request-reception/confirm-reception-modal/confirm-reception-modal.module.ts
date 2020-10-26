import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { ConfirmReceptionModalPageRoutingModule } from './confirm-reception-modal-routing.module';

import { ConfirmReceptionModalPage } from './confirm-reception-modal.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ConfirmReceptionModalPageRoutingModule
  ],
  declarations: [ConfirmReceptionModalPage]
})
export class ConfirmReceptionModalPageModule {}
