import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EventManagementApiClient, Event } from '../services/event-management-api.client';
import { SessionService } from '../services/session.service';

@Component({
  selector: 'app-event-danger-zone',
  templateUrl: './event-danger-zone.component.html',
  styleUrls: ['./event-danger-zone.component.css']
})
export class EventDangerZoneComponent implements OnInit {
  wantDelete: Boolean = false;
  inputValue: string;
  event: Event;

  constructor(
    private apiClient : EventManagementApiClient,
    private session: SessionService,
    private route: ActivatedRoute,
    private router : Router
  ) {}

  async ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.event = <Event>await this.apiClient
        .events_GetEvent(id).toPromise();
    }
  }

  canDelete(): Boolean {
    return this.inputValue === this.event.name;
  }

  async delete() {
    await this.apiClient.events_DeleteEvent(this.event.id).toPromise();
    this.session.unsetCurrentEvent();
    this.router.navigate(['/events'])
  }
}
