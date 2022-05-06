import {RecipeIngredient} from './recipe-ingredient';
import {RecipeStep} from './recipe-step';

export class Recipe {
  id = '';
  name = '';
  description = '';
  photos: string[] = [];
  ingredients: RecipeIngredient[] = [];
  recipeSteps: RecipeStep[] = [];

  isValid(): boolean {
    return this.name
      && this.description
      && this.recipeSteps
      && this.recipeSteps.length > 0
      && this.photos
      && this.photos.length > 0;
  }
}
