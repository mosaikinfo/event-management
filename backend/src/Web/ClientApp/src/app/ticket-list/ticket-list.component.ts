import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LazyLoadEvent } from 'primeng/components/common/api';
import { Event, Ticket, EventManagementApiClient, TicketType } from '../services/event-management-api.client';
import { SessionService } from '../services/session.service';

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
  firstRow: number = 0;
  pageSize: number = 20;
  totalRecords: number;

  searchText: string;
  filter: string;

  constructor(
    private router: Router,
    private session: SessionService,
    private apiClient: EventManagementApiClient
  ) {}

  async ngOnInit() {
    this.event = await this.session.getCurrentEvent();
    await this.loadTicketTypes();
    await this.loadTickets();
  }

  async loadTicketTypes(): Promise<void> {
    this.ticketTypes = await this.apiClient
      .ticketTypes_GetTicketTypes(this.event.id)
      .toPromise();
  }

  async loadTickets(): Promise<void> {
    const eventId = this.event.id;
    this.loading = true;
    let page = this.firstRow / this.pageSize + 1;
    let result = await this.apiClient.tickets_GetTickets(
          eventId,
          this.filter || '',
          '',
          page,
          this.pageSize)
        .toPromise();
    this.tickets = result.data;
    this.totalRecords = result.totalCount;
    this.loading = false;
  }

  onLazyLoad(event: LazyLoadEvent) {
    this.firstRow = event.first;
    if (this.event) {
      this.loadTickets();
    }
  }

  search(): Boolean {
    if (this.searchText) {
      const v = this.searchText;
      this.filter = 
        `ticketNumber~=${v};or$` +
        `firstName~=${v};or$` +
        `lastName~=${v};or$` +
        `roomNumber~=${v}`;
    }
    else {
      this.filter = null;
    }
    this.firstRow = 0;
    this.loadTickets();
    return false;
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
