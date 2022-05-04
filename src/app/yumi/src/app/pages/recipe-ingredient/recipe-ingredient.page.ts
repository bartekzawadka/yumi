import {Component, Input, OnInit} from '@angular/core';
import {RecipeIngredient} from '../../models/recipe-ingredient';
import {ModalController} from "@ionic/angular";

@Component({
  selector: 'app-recipe-ingredient',
  templateUrl: './recipe-ingredient.page.html',
  styleUrls: ['./recipe-ingredient.page.scss'],
})
export class RecipeIngredientPage implements OnInit {
  @Input() data: RecipeIngredient = new RecipeIngredient();
  constructor(private modalController: ModalController) { }

  ngOnInit() {
  }

  async dismissModal() {
    await this.modalController.dismiss();
  }

  async save() {
    await this.modalController.dismiss(this.data);
  }
}
