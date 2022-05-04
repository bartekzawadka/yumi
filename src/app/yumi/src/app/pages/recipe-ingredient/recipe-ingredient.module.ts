import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { RecipeIngredientPageRoutingModule } from './recipe-ingredient-routing.module';

import { RecipeIngredientPage } from './recipe-ingredient.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RecipeIngredientPageRoutingModule
  ],
  declarations: [RecipeIngredientPage]
})
export class RecipeIngredientPageModule {}
