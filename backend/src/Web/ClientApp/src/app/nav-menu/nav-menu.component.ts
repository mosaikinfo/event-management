import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { SessionService } from '../services/session.service';
import { Event, EventManagementApiClient, MailSettings } from '../services/event-management-api.client';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  isExpanded = false;
  currentUser: string = "";
  currentEvent: Event;
  demoModeEnabled: Boolean = false;

  constructor(
    private authService: AuthService,
    private session: SessionService,
    private apiClient: EventManagementApiClient
  ) {}

  async ngOnInit() {
    this.authService.onUserLoggedIn
      .subscribe(claims => {
        this.currentUser = claims['name'];
        this.checkIsDemoMode();
      });
    this.session.onCurrentEventChanged
      .subscribe((evt: Event) => {
        this.currentEvent = evt;
        this.checkIsDemoMode();
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

  async checkIsDemoMode() {
    if (await this.authService.isLoggedIn() && this.currentEvent) {
      this.apiClient.mailSettings_GetMailSettings(this.currentEvent.id)
          .subscribe((settings: MailSettings) => {
            this.demoModeEnabled = settings.enableDemoMode;
          });
    }
  }
}
