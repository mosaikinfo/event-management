import { Component, OnInit } from '@angular/core';
import { Event, Ticket, EventManagementApiClient, TicketType } from '../services/event-management-api.client';
import { SessionService } from '../services/session.service';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})
export class TicketListComponent implements OnInit {
  tickets: Ticket[];
  ticketTypes: TicketType[];

  constructor(
    private session: SessionService,
    private apiClient: EventManagementApiClient
  ) {}

  async ngOnInit() {
    let event = <Event>await this.session.getCurrentEvent();
    this.apiClient.ticketTypes_GetTicketTypes(event.id)
      .subscribe((items: TicketType[]) => this.ticketTypes = items);
    this.apiClient.tickets_GetTickets(event.id)
      .subscribe((tickets: Ticket[]) => this.tickets = tickets);
  }

  getTicketType(ticketTypeId: number): TicketType {
    return this.ticketTypes.find(t => t.id === ticketTypeId);
  }
}
