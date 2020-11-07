import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { AssignUserModalPageRoutingModule } from './assign-user-modal-routing.module';

import { AssignUserModalPage } from './assign-user-modal.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    AssignUserModalPageRoutingModule
  ],
  declarations: [AssignUserModalPage]
})
export class AssignUserModalPageModule {}
