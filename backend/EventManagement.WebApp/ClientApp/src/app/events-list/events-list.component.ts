import { Component, OnInit } from '@angular/core';
import { EventManagementApiClient } from '../services/event-management-api.client';

@Component({
  selector: 'app-events-list',
  templateUrl: './events-list.component.html',
  styleUrls: ['./events-list.component.css']
})
export class EventsListComponent implements OnInit {

  events = [];

  constructor(private apiClient: EventManagementApiClient) { }

  ngOnInit() {
    this.apiClient.events_GetAll()
      .subscribe(events => this.events = events);
  }

}
