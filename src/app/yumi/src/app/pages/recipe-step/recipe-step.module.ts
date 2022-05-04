import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { RecipeStepPageRoutingModule } from './recipe-step-routing.module';

import { RecipeStepPage } from './recipe-step.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RecipeStepPageRoutingModule
  ],
  declarations: [RecipeStepPage]
})
export class RecipeStepPageModule {}
