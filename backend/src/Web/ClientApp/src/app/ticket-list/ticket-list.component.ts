import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LazyLoadEvent, SelectItem } from 'primeng/components/common/api';
import { Event, Ticket, EventManagementApiClient, TicketType } from '../services/event-management-api.client';
import { SessionService } from '../services/session.service';

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})
export class TicketListComponent implements OnInit {
  event: Event;
  tickets: Ticket[];
  selectedTicket: Ticket;
  
  loading: Boolean;
  firstRow: number = 0;
  pageSize: number = 30;
  totalRecords: number;

  optionalSwitchOptions: SelectItem[] = [
    { label: "Beliebig", value: undefined },
    { label: "Ja", value: true },
    { label: "Nein", value: false }
  ];
  filterDelivered: boolean;
  filterOnlyValidated: boolean;

  ticketTypeOptions: SelectItem[];
  selectedTicketType: any;

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
    let ticketTypes = await this.apiClient
      .ticketTypes_GetTicketTypes(this.event.id)
      .toPromise();
    this.ticketTypeOptions = [{ label: "Alle", value: undefined }];
    for(let ticketType of ticketTypes) {
      this.ticketTypeOptions.push({
        label: ticketType.name, 
        value: ticketType.id
      });
    }
  }

  async loadTickets(): Promise<void> {
    if (!this.event)
      return;
    const eventId = this.event.id;
    this.loading = true;
    let page = this.firstRow / this.pageSize + 1;
    let filter = this._buildFilter();
    let result = await this.apiClient.tickets_GetTickets(
          eventId,
          filter,
          undefined,
          page,
          this.pageSize,
          this.filterDelivered,
          this.filterOnlyValidated,
          this.selectedTicketType)
        .toPromise();
    this.tickets = result.data;
    this.totalRecords = result.totalCount;
    this.loading = false;
  }

  _buildFilter(): string {
    return this.filter || undefined;
  }

  pTable_onLazyLoad(event: LazyLoadEvent) {
    this.firstRow = event.first;
    this.pageSize = event.rows;
    this.loadTickets();
  }

  search(): Boolean {
    if (this.searchText) {
      const v = this.searchText;
      this.filter = 
        `ticketNumber~=${v};` +
        `firstName~=${v};` +
        `lastName~=${v};` +
        `roomNumber~=${v};or`;
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
}
