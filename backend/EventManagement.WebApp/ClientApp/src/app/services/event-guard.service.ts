import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import { SessionService } from './session.service';
import { Event } from './event-management-api.client';

/*
 * Router guard to ensure that the current user has selected an event.
 */
@Injectable({
  providedIn: 'root'
})
export class EventGuardService implements CanActivate {

  constructor(
    private session: SessionService,
    private router: Router
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return this.session.getCurrentEvent().then((evt: Event) => {
      if (evt) {
        return true;
      }
      this.router.navigate(["/events"]);
      return false;
    });
  }
}
