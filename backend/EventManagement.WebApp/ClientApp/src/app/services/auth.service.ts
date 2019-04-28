import { Constants } from '../../constants';
import { Injectable, Output, EventEmitter } from '@angular/core';
import { UserManager, UserManagerSettings, User } from 'oidc-client';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private _userManager: UserManager;

  @Output() onUserLoggedIn = new EventEmitter<any>();

  constructor(private router: Router) {
    let settings = {
        authority: Constants.stsAuthority,
        client_id: Constants.clientId,
        redirect_uri: `${Constants.clientRoot}/auth-callback`,
        silent_redirect_uri: `${Constants.clientRoot}/silent-refresh.html`,
        automaticSilentRenew: true,
        post_logout_redirect_uri: Constants.clientRoot,
        response_type: 'id_token token',
        scope: Constants.clientScope,
        filterProtocolClaims: true,
        loadUserInfo: true,
    };
    this._userManager = new UserManager(settings);
  }

  public getUser(): Promise<User> {
    return this._userManager.getUser();
  }

  isLoggedIn(): Promise<boolean> {
    return this.getUser()
      .then(user => user != null && !user.expired);
  }

  getAuthorizationHeaderValue(): Promise<string> {
    return this.getUser()
      .then(user => `${user.token_type} ${user.access_token}`)
  }

  login(currentUrl: string = null): Promise<void> {
    this.persistState(currentUrl);
    return this._userManager.signinRedirect();
  }

  logout(): Promise<void> {
    return this._userManager.signoutRedirect();
  }

  completeAuthentication(): Promise<void> {
    return this._userManager.signinRedirectCallback().then(user => {
      this.onUserLoggedIn.emit(user.profile);
      this.restoreState();
    });
  }

  private persistState(currentUrl: string) {
    if (currentUrl) {
      window.sessionStorage.setItem("currentUrl", currentUrl);
    }
  }

  private restoreState() {
    let currentUrl = window.sessionStorage.getItem("currentUrl");
    if (currentUrl) {
      this.router.navigateByUrl(currentUrl);
    }
  }
}