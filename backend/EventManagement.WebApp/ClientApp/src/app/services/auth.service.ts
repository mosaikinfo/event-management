import { Injectable, Output, EventEmitter } from '@angular/core';
import { UserManager, UserManagerSettings, User } from 'oidc-client';

@Injectable()
export class AuthService {

  private manager = new UserManager(getClientSettings());
  private user: User = null;

  @Output() onUserLoggedIn = new EventEmitter<any>();

  constructor() {
    this.manager.getUser().then(user => {
      this.user = user;
    });
  }

  isLoggedIn(): boolean {
    return this.user != null && !this.user.expired;
  }

  getClaims(): any {
    return this.user.profile;
  }

  getAuthorizationHeaderValue(): string {
    return `${this.user.token_type} ${this.user.access_token}`;
  }

  startAuthentication(): Promise<void> {
    return this.manager.signinRedirect();
  }

  startLogout(): Promise<void> {
    return this.manager.signoutRedirect();
  }

  completeAuthentication(): Promise<void> {
    return this.manager.signinRedirectCallback().then(user => {
      this.user = user;
      this.onUserLoggedIn.emit(user.profile);
    });
  }
}

export function getClientSettings(): UserManagerSettings {
  return {
      authority: 'http://localhost:5000',
      client_id: 'admin-app',
      redirect_uri: 'http://localhost:5000/auth-callback',
      post_logout_redirect_uri: 'http://localhost:5000/',
      response_type:"id_token token",
      scope:"openid profile eventmanagement.admin",
      filterProtocolClaims: true,
      loadUserInfo: true,
      automaticSilentRenew: true,
      silent_redirect_uri: 'http://localhost:5000/silent-refresh.html'
  };
}