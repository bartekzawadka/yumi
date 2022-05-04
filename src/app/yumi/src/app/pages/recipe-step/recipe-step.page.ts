import {Component, Input, OnInit} from '@angular/core';
import {RecipeStep} from '../../models/recipe-step';
import {ModalController} from "@ionic/angular";

@Component({
  selector: 'app-recipe-step',
  templateUrl: './recipe-step.page.html',
  styleUrls: ['./recipe-step.page.scss'],
})
export class RecipeStepPage implements OnInit {
  @Input() data: RecipeStep = new RecipeStep();
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
