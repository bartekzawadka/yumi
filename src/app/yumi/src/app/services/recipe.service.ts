import {Injectable} from '@angular/core';
import {ApiService} from './api.service';
import {HttpClient} from '@angular/common/http';
import {Recipe} from '../models/recipe';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecipeService extends ApiService {
  root = 'recipe';

  constructor(protected http: HttpClient) {
    super(http);
  }

  createRecipe(recipe: Recipe): Observable<any> {
    return this.post<Recipe, any>('', recipe);
  }
}
