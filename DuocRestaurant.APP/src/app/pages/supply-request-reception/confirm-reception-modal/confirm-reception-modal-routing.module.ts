import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ConfirmReceptionModalPage } from './confirm-reception-modal.page';

const routes: Routes = [
  {
    path: '',
    component: ConfirmReceptionModalPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ConfirmReceptionModalPageRoutingModule {}
