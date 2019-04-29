import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SessionService } from '../services/session.service';
import { Event } from '../services/event-management-api.client';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent {

  constructor(
    private router: Router,
    private session: SessionService
  ) {
    this.session.getCurrentEvent()
    .then((evt: Event) => {
      if (evt == null) {
        this.router.navigate(["/events"]);
      }
    });
  }
}
