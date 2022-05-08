import { HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {Injectable, Injector} from '@angular/core';
import {AuthService} from '../services/auth.service';
import {catchError} from 'rxjs/operators';
import {Router} from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor{
  constructor(private injector: Injector) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
    const authService = this.injector.get(AuthService);
    req = req.clone({headers: req.headers.set('Authorization', 'Bearer ' + authService.getUserData().token)});

    return next.handle(req)
      .pipe(
        catchError(err => {
          const router = this.injector.get(Router);
          if(err.status === 401 && !err.url.endsWith('auth')){
            authService.logOff();
            return router.navigate(['/login']);
          }else{
            return throwError(err);
          }
        })
      );
  }
}
