import { Component, OnInit } from '@angular/core';
import { EventManagementApiClient, Event } from '../services/event-management-api.client';
import { Router } from '@angular/router';

@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.css']
})
export class EventFormComponent implements OnInit {

  model : Event = new Event();
  minDate = new Date();

  constructor(
    private apiClient : EventManagementApiClient,
    private router: Router 
  ) {}

  ngOnInit() {
  }

  submit() {
    this.apiClient.events_CreateEvent(this.model)
      .subscribe(() => this.router.navigate(['/events']));
  }
}
