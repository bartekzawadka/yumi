import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'recipes',
    pathMatch: 'full'
  },
  {
    path: 'recipes',
    loadChildren: () => import('./pages/recipes/recipes.module').then( m => m.RecipesPageModule)
  },
  {
    path: 'recipe',
    loadChildren: () => import('./pages/recipe/recipe.module').then( m => m.RecipePageModule)
  },
  {
    path: 'recipe-step',
    loadChildren: () => import('./pages/recipe-step/recipe-step.module').then( m => m.RecipeStepPageModule)
  },
  {
    path: 'recipe-ingredient',
    loadChildren: () => import('./pages/recipe-ingredient/recipe-ingredient.module').then( m => m.RecipeIngredientPageModule)
  },
  {
    path: 'recipe-preview',
    loadChildren: () => import('./pages/recipe-preview/recipe-preview.module').then( m => m.RecipePreviewPageModule)
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
