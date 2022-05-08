export class TokenData {
  token = '';

  isAuthenticated(): boolean {
    return !!this.token;
  }
}
