import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AvailabilityPage } from './availability.page';

const routes: Routes = [
  {
    path: '',
    component: AvailabilityPage
  },  {
    path: 'assign-user-modal',
    loadChildren: () => import('./assign-user-modal/assign-user-modal.module').then( m => m.AssignUserModalPageModule)
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AvailabilityPageRoutingModule {}
