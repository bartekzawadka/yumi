import {Component, OnInit} from '@angular/core';
import {Recipe} from '../../models/recipe';
import {PageBase} from '../page.base';
import {ActivatedRoute} from '@angular/router';
import {AlertController, LoadingController} from '@ionic/angular';
import {RecipeService} from '../../services/recipe.service';
import * as moment from 'moment';

@Component({
  selector: 'app-recipe-preview',
  templateUrl: './recipe-preview.page.html',
  styleUrls: ['./recipe-preview.page.scss'],
})
export class RecipePreviewPage extends PageBase implements OnInit {
  data = new Recipe();
  slidesOptions = {
    autoHeight: true,
    centerSlides: true,
  };

  constructor(private activatedRoute: ActivatedRoute,
              protected loadingController: LoadingController,
              protected alertController: AlertController,
              private recipeService: RecipeService) {
    super(loadingController, alertController);
  }

  ngOnInit() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    this.callWithLoader(() => this.recipeService.getById(id))
      .then(value => {
        const data = new Recipe();
        data.recipeSteps = value.recipeSteps;
        data.photos = value.photos;
        data.ingredients = value.ingredients;
        data.id = value.id;
        data.name = value.name;
        data.description = value.description;
        this.data = data;
      })
      .catch(err => this.showError(err));
  }

  getIdleTimeSum() {
    let sum = 0;
    this.data.recipeSteps.forEach(value => {
      sum += value.idleTimeInMinutes;
    });
    return sum;
  }

  getIdleDisplayValue(idleTimeInMinutes: number) {
    return moment.duration(idleTimeInMinutes, 'minutes').humanize();
  }
}
