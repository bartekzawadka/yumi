import { Injectable } from '@angular/core';
import {ApiService} from './api.service';
import {HttpClient} from '@angular/common/http';
import {TokenData} from '../models/auth/token-data';
import {Observable} from 'rxjs';
import {tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends ApiService {
  root = 'auth';
  private tokenStorageKey = 'yumi.user.data';
  private userData = new TokenData();

  constructor(protected http: HttpClient) {
    super(http);
  }

  getUserData(): TokenData {
    if (this.userData.isAuthenticated()) {
      return this.userData;
    }

    this.userData = new TokenData();
    const value = localStorage.getItem(this.tokenStorageKey);
    if (value) {
      const user = JSON.parse(value);
      this.copyTokenLocal(user);
    }

    return this.userData;
  }

  authenticate(token: string): Observable<TokenData>{
    return this.get<TokenData>('?token=' + token).pipe(
      tap(value => {
        this.storeToken(value);
      })
    );
  }

  logOff(): void {
    this.userData = new TokenData();
    localStorage.removeItem(this.tokenStorageKey);
  }

  private storeToken(value: TokenData): void {
    if (!value) {
      return;
    }

    localStorage.setItem(this.tokenStorageKey, JSON.stringify(value));
    this.copyTokenLocal(value);
  }

  private copyTokenLocal(value: TokenData): void {
    const data = new TokenData();
    data.token = value.token;
    this.userData = data;
  }
}
