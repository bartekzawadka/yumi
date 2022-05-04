import {RecipeIngredient} from './recipe-ingredient';
import {RecipeStep} from './recipe-step';

export class Recipe {
  id = '';
  name = '';
  description = '';
  ingredients: RecipeIngredient[] = [];
  recipeSteps: RecipeStep[] = [];

  isValid(): boolean {
    return this.name && this.description && this.recipeSteps && this.recipeSteps.length > 0;
  }
}
