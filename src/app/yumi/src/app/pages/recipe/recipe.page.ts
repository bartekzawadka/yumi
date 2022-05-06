import {Component, OnInit} from '@angular/core';
import {RecipeIngredient} from '../../models/recipe-ingredient';
import {RecipeStep} from '../../models/recipe-step';
import {AlertController, LoadingController, ModalController} from '@ionic/angular';
import {RecipeStepPage} from '../recipe-step/recipe-step.page';
import {RecipeIngredientPage} from '../recipe-ingredient/recipe-ingredient.page';
import {Recipe} from 'src/app/models/recipe';
import * as moment from 'moment';
import {RecipeService} from '../../services/recipe.service';
import {PageBase} from '../page.base';
import {Router} from '@angular/router';

@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.page.html',
  styleUrls: ['./recipe.page.scss'],
})
export class RecipePage extends PageBase implements OnInit {
  model = new Recipe();
  slidesOptions = {
    autoHeight: true,
    centerSlides: true,
  };

  constructor(
    private router: Router,
    protected loadingController: LoadingController,
    protected alertController: AlertController,
    private modalController: ModalController,
    private recipeService: RecipeService) {
    super(loadingController, alertController);
  }

  ngOnInit() {
  }

  async addRecipeIngredient() {
    const modal = await this.modalController.create({
      component: RecipeIngredientPage,
      mode: 'ios',
      componentProps: {
        data: new RecipeIngredient(),
      },
      canDismiss: true,
    });

    await modal.present();
    modal.onDidDismiss().then(result => {
      if (!result.data) {
        return;
      }

      this.model.ingredients.push(result.data as RecipeIngredient);
    });
  }

  async addRecipeStep() {
    const modal = await this.modalController.create({
      component: RecipeStepPage,
      cssClass: 'fullscreen',
      mode: 'ios',
      componentProps: {
        data: new RecipeStep(),
      },
      canDismiss: true,
    });

    await modal.present();
    modal.onDidDismiss().then(result => {
      if (!result.data) {
        return;
      }

      this.model.recipeSteps.push(result.data as RecipeStep);
    });
  }

  getIdleDisplayValue(idleTimeInMinutes: number) {
    return moment.duration(idleTimeInMinutes, 'minutes').humanize();
  }

  saveRecipe() {
    this.callWithLoader(() => this.recipeService.createRecipe(this.model))
      .then(() => {
        this.router.navigate(['/']);
      })
      .catch(error => this.showError(error));
  }

  async editStep(step: RecipeStep, index: number) {
    const data = new RecipeStep();
    data.idleTimeInMinutes = step.idleTimeInMinutes;
    data.name = step.name;
    data.content = step.content;
    const modal = await this.modalController.create({
      component: RecipeStepPage,
      cssClass: 'fullscreen',
      mode: 'ios',
      componentProps: {
        data,
      },
      canDismiss: true,
    });

    await modal.present();
    modal.onDidDismiss().then(result => {
      if (!result.data) {
        return;
      }

      this.model.recipeSteps[index] = result.data as RecipeStep;
    });
  }

  deleteStep(i: number) {
    this.model.recipeSteps.splice(i, 1);
  }

  deleteIngredient(i: number) {
    this.model.ingredients.splice(i, 1);
  }

  onImageChange(e) {
    const reader = new FileReader();

    if (e.target.files && e.target.files.length) {
      const [file] = e.target.files;
      reader.readAsDataURL(file);

      reader.onload = () => {
        this.model.photos.push(reader.result as string);
      };
    }
  }

  addImage() {
    document.getElementById('fileInput').click();
  }
}
