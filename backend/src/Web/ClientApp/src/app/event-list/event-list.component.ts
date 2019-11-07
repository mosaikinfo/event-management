import { Component, OnInit } from '@angular/core';
import { EventManagementApiClient, Event } from '../services/event-management-api.client';
import { SessionService } from '../services/session.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})
export class EventListComponent implements OnInit {

  events = [];

  constructor(
    private router: Router,
    private apiClient: EventManagementApiClient,
    private session: SessionService) { }

  ngOnInit() {
    this.apiClient.events_GetAll()
      .subscribe(events => this.events = events);
  }

  select(evt: Event, navigate: Boolean) {
    this.session.setCurrentEvent(evt);
    if (navigate) {
      this.router.navigate(['/']);
    }
  }
}
