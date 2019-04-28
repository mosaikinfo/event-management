import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { SessionService } from '../services/session.service';
import { Event } from '../services/event-management-api.client';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  currentUser: string = "";
  currentEvent: Event;

  constructor(
    private authService: AuthService,
    private session: SessionService
  ) {
   this.authService.onUserLoggedIn
      .subscribe(claims => this.currentUser = claims['name']);
    this.session.onCurrentEventChanged
      .subscribe((evt: Event) => this.currentEvent = evt);
  }

  logout() {
    this.authService.logout();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
