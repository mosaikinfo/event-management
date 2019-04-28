import { Component, OnInit } from '@angular/core';
import { EventManagementApiClient, Event } from '../services/event-management-api.client';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.css']
})
export class EventFormComponent implements OnInit {

  model : Event = new Event();

  constructor(
    private apiClient : EventManagementApiClient,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    const id = +this.route.snapshot.paramMap.get('id');
    if (id) {
      this.apiClient.events_GetEvent(id)
        .subscribe((evt: Event) => this.model = evt);
    }
  }

  submit() {
    if (this.model.id > 0) {
      this.apiClient.events_UpdateEvent(this.model.id, this.model)
        .subscribe(() => this.router.navigate(['/events']));
    } else {
      this.apiClient.events_CreateEvent(this.model)
        .subscribe(() => this.router.navigate(['/events']));
    }
  }
}
