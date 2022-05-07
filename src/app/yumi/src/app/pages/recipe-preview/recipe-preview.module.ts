import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { RecipePreviewPageRoutingModule } from './recipe-preview-routing.module';

import { RecipePreviewPage } from './recipe-preview.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RecipePreviewPageRoutingModule
  ],
  declarations: [RecipePreviewPage]
})
export class RecipePreviewPageModule {}
