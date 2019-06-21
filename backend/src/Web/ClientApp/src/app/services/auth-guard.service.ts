import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import { AuthService } from '../services/auth.service';

/*
 * Router guard to ensure that the current user is authenticated.
 */
@Injectable({
    providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

    constructor(private authService: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
        return this.authService.isLoggedIn().then(isLoggedIn => {
            if(isLoggedIn) {
                return true;
            }
            this.authService.login(state.url);
            return false;
        });
    }
}