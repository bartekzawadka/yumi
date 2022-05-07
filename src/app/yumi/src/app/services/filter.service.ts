import { Injectable } from '@angular/core';
import {GetRecipesListQuery} from '../models/queries/get-recipes-list-query';

@Injectable({
  providedIn: 'root'
})
export class FilterService {
  recipesQueryFilter = new GetRecipesListQuery();

  constructor() { }
}
