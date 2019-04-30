import { Component, OnInit } from '@angular/core';
import { EventManagementApiClient, Event } from '../services/event-management-api.client';
import { ActivatedRoute } from '@angular/router';
import { PageAlertService } from '../page-alert/page-alert.service';

@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.css']
})
export class EventFormComponent implements OnInit {

  model : Event = new Event();

  constructor(
    private apiClient : EventManagementApiClient,
    private route: ActivatedRoute,
    private alertService: PageAlertService
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
        .subscribe(() => this.alertService.showSaveSuccessAlert());
    } else {
      this.apiClient.events_CreateEvent(this.model)
        .subscribe(() => this.alertService.showSaveSuccessAlert());
    }
  }
}
