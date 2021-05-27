import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
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
  showPastEvents = false;

  constructor(
    private router: Router,
    private apiClient: EventManagementApiClient,
    private session: SessionService) { }

  ngOnInit() {
    this.reloadData();
  }

  reloadData() {
    const future = this.showPastEvents ? undefined : true;
    this.apiClient.events_List(null, future)
      .subscribe(events => this.events = events);
  }

  select(evt: Event, navigate: Boolean) {
    this.session.setCurrentEvent(evt);
    if (navigate) {
      this.router.navigate(['/']);
    }
  }
}
