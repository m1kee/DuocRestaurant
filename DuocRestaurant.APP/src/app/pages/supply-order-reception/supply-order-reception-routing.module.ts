import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SupplyOrderReceptionPage } from './supply-order-reception.page';

const routes: Routes = [
  {
    path: '',
    component: SupplyOrderReceptionPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SupplyOrderReceptionPageRoutingModule {}
