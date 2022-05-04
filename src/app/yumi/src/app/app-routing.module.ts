import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'folder/Inbox',
    pathMatch: 'full'
  },
  {
    path: 'folder/:id',
    loadChildren: () => import('./folder/folder.module').then( m => m.FolderPageModule)
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
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
