import { Component, OnInit } from '@angular/core';
import { Event, Ticket, EventManagementApiClient } from '../services/event-management-api.client';
import { SessionService } from '../services/session.service';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})
export class TicketListComponent implements OnInit {
  tickets: Ticket[];

  constructor(
    private session: SessionService,
    private apiClient: EventManagementApiClient
  ) { 
    this.session.getCurrentEvent()
      .then((evt: Event) => {
        this.apiClient.tickets_GetTickets(evt.id)
          .subscribe((tickets: Ticket[]) => this.tickets = tickets);
    });
  }

  ngOnInit() {
  }

}
