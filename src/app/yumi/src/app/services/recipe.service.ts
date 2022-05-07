import {Injectable} from '@angular/core';
import {ApiService} from './api.service';
import {HttpClient} from '@angular/common/http';
import {Recipe} from '../models/recipe';
import {Observable} from 'rxjs';
import {PagedList} from "../models/paged-list";
import {RecipeListItem} from "../models/recipe-list-item";
import {FilterService} from "./filter.service";
import {GetRecipesListQuery} from "../models/queries/get-recipes-list-query";

@Injectable({
  providedIn: 'root'
})
export class RecipeService extends ApiService {
  root = 'recipe';

  constructor(
    protected http: HttpClient,
    private filterService: FilterService) {
    super(http);
  }

  getRecipes(): Observable<PagedList<RecipeListItem>> {
    return this.post<GetRecipesListQuery, PagedList<RecipeListItem>>('/search', this.filterService.recipesQueryFilter);
  }

  getById(id: string): Observable<Recipe> {
    return this.get<Recipe>('/' + id);
  }

  createRecipe(recipe: Recipe): Observable<any> {
    return this.post<Recipe, any>('', recipe);
  }

  editRecipe(recipe: Recipe): Observable<any> {
    return this.put<Recipe>('', recipe);
  }

  deleteRecipe(id: string): Observable<any> {
    return this.delete(id);
  }
}
