import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RecipeIngredientPage } from './recipe-ingredient.page';

const routes: Routes = [
  {
    path: '',
    component: RecipeIngredientPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RecipeIngredientPageRoutingModule {}
