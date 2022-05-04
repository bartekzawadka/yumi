import {AlertController, LoadingController} from '@ionic/angular';
import {Observable} from 'rxjs';

export abstract class PageBase {
  protected constructor(
    protected loadingController: LoadingController,
    protected alertController: AlertController) {
  }

  async callWithLoader<T>(action: () => Observable<T>) {
    const loading = await this.loadingController.create();
    await loading.present();

    try {
      return await action().toPromise();
    } catch (error) {
      throw error;
    } finally {
      await loading.dismiss();
    }
  }

  async showError(error: any, title?: string){
    let message = this.parseError(error);
    if(!message){
      console.log(error);
      message = 'Więcej szczegółów w logach';
    }

    const alert = await this.alertController.create({
      header: title,
      message,
      buttons: ['OK']
    });
    await alert.present();
  }

  protected parseError(err: any): string {
    if (err && err.error && err.error.errors && Array.isArray(err.error.errors)) {
      if (err.error.errors.length === 1) {
        return err.error.errors[0];
      }

      let msg = '';
      err.error.errors.forEach(i => msg += i + '');
      return msg;
    } else if (err && err.error && err.error.errors) {
      let msg = '';
      for (const k in err.error.errors) {
        if (err.error.errors.hasOwnProperty(k)) {
          msg += err.error.errors[k] + ' ';
        }
      }

      return msg;
    } else if ((typeof err === 'string' || err instanceof String)) {
      return err?.toString();
    }

    return "Unexpected error occurred";
  }
}
