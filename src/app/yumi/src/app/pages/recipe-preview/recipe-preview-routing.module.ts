import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RecipePreviewPage } from './recipe-preview.page';

const routes: Routes = [
  {
    path: ':id',
    component: RecipePreviewPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RecipePreviewPageRoutingModule {}
