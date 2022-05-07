import {AfterViewInit, Component, OnInit} from '@angular/core';
import {FilterService} from '../../services/filter.service';
import {RecipeService} from '../../services/recipe.service';
import {PageBase} from '../page.base';
import {AlertController, LoadingController} from '@ionic/angular';
import {RecipeListItem} from '../../models/recipe-list-item';
import {Router} from '@angular/router';

@Component({
  selector: 'app-recipes',
  templateUrl: './recipes.page.html',
  styleUrls: ['./recipes.page.scss'],
})
export class RecipesPage extends PageBase implements OnInit, AfterViewInit {
  data: RecipeListItem[] = [];

  constructor(
    private router: Router,
    public filterService: FilterService,
    private recipeService: RecipeService,
    protected loadingController: LoadingController,
    protected alertController: AlertController,) {
    super(loadingController, alertController);
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
  }

  ionViewDidEnter() {
    this.initialize();
    this.loadData();
  }

  initialize() {
    this.filterService.recipesQueryFilter.pageSize = 10;
    this.filterService.recipesQueryFilter.pageIndex = 0;
    this.data = [];
  }

  loadData() {
    this.callWithLoader(() => this.recipeService.getRecipes())
      .then(recipes => {
        this.filterService.recipesQueryFilter.pageSize = recipes.pageSize;
        this.filterService.recipesQueryFilter.pageIndex = recipes.pageIndex;
        this.data = [...this.data, ...recipes.data];
      })
      .catch(err => this.showError(err));
  }

  preview() {

  }

  async editRecipe(id: string) {
    await this.router.navigate(['/recipe/' + id]);
  }

  async deleteRecipe(id: string) {
    await this.showConfirmation(
      'Confirmation',
      'Are you sure you want to remove this item?',
      () => {
        this.deleteConfirmed(id);
      });
  }

  deleteConfirmed(id: string) {
    this.callWithLoader(() => this.recipeService.deleteRecipe(id))
      .then(() => {
        this.initialize();
        this.loadData();
      })
      .catch(err => this.showError(err));
  }

  onSearchPhraseChange() {
    this.initialize();
    this.loadData();
  }

  refresh($event: any) {
    this.initialize();
    this.loadData();
    $event.target.complete();
  }
}
