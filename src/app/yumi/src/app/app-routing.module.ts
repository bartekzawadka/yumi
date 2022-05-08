import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import {AuthGuard} from "./auth/auth-guard";

const routes: Routes = [
  {
    path: '',
    redirectTo: 'recipes',
    pathMatch: 'full'
  },
  {
    path: 'recipes',
    canActivate: [AuthGuard],
    loadChildren: () => import('./pages/recipes/recipes.module').then( m => m.RecipesPageModule)
  },
  {
    path: 'recipe',
    canActivate: [AuthGuard],
    loadChildren: () => import('./pages/recipe/recipe.module').then( m => m.RecipePageModule)
  },
  {
    path: 'recipe-step',
    canActivate: [AuthGuard],
    loadChildren: () => import('./pages/recipe-step/recipe-step.module').then( m => m.RecipeStepPageModule)
  },
  {
    path: 'recipe-ingredient',
    canActivate: [AuthGuard],
    loadChildren: () => import('./pages/recipe-ingredient/recipe-ingredient.module').then( m => m.RecipeIngredientPageModule)
  },
  {
    path: 'recipe-preview',
    canActivate: [AuthGuard],
    loadChildren: () => import('./pages/recipe-preview/recipe-preview.module').then( m => m.RecipePreviewPageModule)
  },
  {
    path: 'login',
    loadChildren: () => import('./pages/login/login.module').then( m => m.LoginPageModule)
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
