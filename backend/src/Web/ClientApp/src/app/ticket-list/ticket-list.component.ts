import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LazyLoadEvent } from 'primeng/components/common/api';
import { Event, Ticket, EventManagementApiClient, TicketType, PaginationResultOfTicket } from '../services/event-management-api.client';
import { SessionService } from '../services/session.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})
export class TicketListComponent implements OnInit {
  event: Event;
  ticketTypes: TicketType[];
  tickets: Ticket[];
  selectedTicket: Ticket;
  loading: Boolean;
  pageSize: number = 10;
  totalRecords: number;

  constructor(
    private router: Router,
    private session: SessionService,
    private apiClient: EventManagementApiClient
  ) {}

  async ngOnInit() {
    this.event = <Event>await this.session.getCurrentEvent();
    await Promise.all([
      this.loadTicketTypes(),
      this.loadTickets(null)
    ]);
  }

  loadTicketTypes(): Subscription {
    return this.apiClient.ticketTypes_GetTicketTypes(this.event.id)
        .subscribe((items: TicketType[]) => this.ticketTypes = items);
  }

  async loadTickets(event: LazyLoadEvent): Promise<void> {
    const eventId = this.event.id;
    this.loading = true;
    let page = (event && event.first || 0) / this.pageSize + 1;
    let result = <PaginationResultOfTicket>await
      this.apiClient
        .tickets_GetTickets(eventId, '', '', page, this.pageSize)
        .toPromise();
    this.tickets = result.data;
    this.totalRecords = result.totalCount;
    this.loading = false;
  }

  edit() {
    if (this.selectedTicket) {
      this.router.navigate(["/tickets", this.selectedTicket.id]);
    }
  }

  getTicketType(ticketTypeId: string): TicketType {
    if (this.ticketTypes) {
      return this.ticketTypes.find(t => t.id === ticketTypeId);
    }
    return null;
  }
}
