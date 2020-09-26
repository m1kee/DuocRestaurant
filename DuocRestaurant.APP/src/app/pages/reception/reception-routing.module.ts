import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReceptionPage } from './reception.page';

const routes: Routes = [
  {
    path: '',
    component: ReceptionPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReceptionPageRoutingModule {}
