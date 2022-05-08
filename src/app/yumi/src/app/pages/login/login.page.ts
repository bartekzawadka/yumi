import {Component, OnInit} from '@angular/core';
import {GoogleLoginProvider, SocialAuthService} from '@abacritt/angularx-social-login';
import {PageBase} from '../page.base';
import {AlertController, LoadingController} from '@ionic/angular';
import {AuthService} from '../../services/auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage extends PageBase implements OnInit {

  constructor(private router: Router,
              protected loadingController: LoadingController,
              protected alertController: AlertController,
              private socialAuthService: SocialAuthService,
              private authService: AuthService) {
    super(loadingController, alertController);
  }

  ngOnInit() {
    this.socialAuthService.authState.subscribe(value => {
      this.authService.authenticate(value.authToken).subscribe(value1 => {
          console.log(value1);
          this.router.navigate(['/']);
        },
        error => this.showError(error, 'Login failed'));
    });
  }

  login() {
    this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID)
      .catch(reason => {
        this.showError(reason, 'Login failed').finally();
      });
  }
}
