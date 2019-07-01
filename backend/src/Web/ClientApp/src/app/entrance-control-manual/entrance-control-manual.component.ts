import { Component, OnInit } from '@angular/core';
import { SessionService } from '../services/session.service';
import { EventManagementApiClient, Event } from '../services/event-management-api.client';
import { PageAlertService } from '../page-alert/page-alert.service';

@Component({
  selector: 'app-entrance-control-manual',
  templateUrl: './entrance-control-manual.component.html',
  styleUrls: ['./entrance-control-manual.component.css']
})
export class EntranceControlManualComponent implements OnInit {
  event: Event;
  ticketNumber: string;

  constructor(
    private session: SessionService,
    private apiClient: EventManagementApiClient,
    private alertService: PageAlertService
  ) { }

  async ngOnInit() {
    this.event = <Event>await this.session.getCurrentEvent();
  }

  validateTicket() {
    this.alertService.showNotImplemented();
  }
}
