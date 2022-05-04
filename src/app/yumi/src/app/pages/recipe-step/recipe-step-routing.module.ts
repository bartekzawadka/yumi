import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RecipeStepPage } from './recipe-step.page';

const routes: Routes = [
  {
    path: '',
    component: RecipeStepPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RecipeStepPageRoutingModule {}
