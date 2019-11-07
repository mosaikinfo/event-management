import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { SessionService } from '../services/session.service';
import { Event, EventManagementApiClient, MailSettings } from '../services/event-management-api.client';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  currentUser: string = "";
  currentEvent: Event;
  demoModeEnabled: Boolean = false;

  constructor(
    private authService: AuthService,
    private session: SessionService,
    private apiClient: EventManagementApiClient
  ) {
   this.authService.onUserLoggedIn
      .subscribe(claims => this.currentUser = claims['name']);
    this.session.onCurrentEventChanged
      .subscribe((evt: Event) => {
        this.currentEvent = evt;
        this.checkDemoMode();
      });
    this.session.onDemoModeChanged
      .subscribe((demoModelEnabled: Boolean) => {
        this.demoModeEnabled = demoModelEnabled;
      });
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

  checkDemoMode() {
    if (this.currentEvent) {
      this.apiClient.mailSettings_GetMailSettings(this.currentEvent.id)
          .subscribe((settings: MailSettings) => {
            this.demoModeEnabled = settings.enableDemoMode;
          });
    }
  }
}
