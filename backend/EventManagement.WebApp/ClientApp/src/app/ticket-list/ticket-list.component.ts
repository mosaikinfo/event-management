import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Event, Ticket, EventManagementApiClient, TicketType } from '../services/event-management-api.client';
import { SessionService } from '../services/session.service';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})
export class TicketListComponent implements OnInit {
  tickets: Ticket[];
  selectedTicket: Ticket;
  ticketTypes: TicketType[];

  constructor(
    private router: Router,
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

  edit() {
    if (this.selectedTicket) {
      this.router.navigate(["/tickets", this.selectedTicket.id]);
    }
  }

  getTicketType(ticketTypeId: number): TicketType {
    if (this.ticketTypes) {
      return this.ticketTypes.find(t => t.id === ticketTypeId);
    }
    return null;
  }
}
