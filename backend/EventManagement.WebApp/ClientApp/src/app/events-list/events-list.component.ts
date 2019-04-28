import { Component, OnInit } from '@angular/core';
import { EventManagementApiClient, Event } from '../services/event-management-api.client';
import { SessionService } from '../services/session.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-events-list',
  templateUrl: './events-list.component.html',
  styleUrls: ['./events-list.component.css']
})
export class EventsListComponent implements OnInit {

  events = [];

  constructor(
    private router: Router,
    private apiClient: EventManagementApiClient,
    private session: SessionService) { }

  ngOnInit() {
    this.apiClient.events_GetAll()
      .subscribe(events => this.events = events);
  }

  select(evt: Event) {
    this.session.setCurrentEvent(evt);
    this.router.navigate(['/']);
  }
}
