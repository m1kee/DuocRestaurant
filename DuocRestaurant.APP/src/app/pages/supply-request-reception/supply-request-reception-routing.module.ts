import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SupplyRequestReceptionPage } from './supply-request-reception.page';

const routes: Routes = [
  {
    path: '',
    component: SupplyRequestReceptionPage
  },
  {
    path: 'confirm-reception-modal',
    loadChildren: () => import('./confirm-reception-modal/confirm-reception-modal.module').then( m => m.ConfirmReceptionModalPageModule)
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SupplyRequestReceptionPageRoutingModule {}
