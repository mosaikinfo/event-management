import { Component, OnInit } from '@angular/core';
import { SessionService } from '../services/session.service';
import { EventManagementApiClient, Event } from '../services/event-management-api.client';

@Component({
  selector: 'app-entrance-control',
  templateUrl: './entrance-control.component.html',
  styleUrls: ['./entrance-control.component.css']
})
export class EntranceControlComponent implements OnInit {
  event: Event;
  ticketNumber: string;

  constructor(
    private session: SessionService,
    private apiClient: EventManagementApiClient
  ) { }

  async ngOnInit() {
    this.event = <Event>await this.session.getCurrentEvent();
  }

  validateTicket() {

  }

}
