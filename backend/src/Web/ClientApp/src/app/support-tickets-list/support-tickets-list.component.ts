import { Component, OnInit, OnDestroy } from '@angular/core';
import { SessionService } from '../services/session.service';
import { EventManagementApiClient, Event, SupportTicket, SupportTicketStatus, SetStatusCommandArgs } from '../services/event-management-api.client';
import { Router } from '@angular/router';
import { orderBy } from 'lodash';

const REFRESH_INTERVAL = 5000;

@Component({
  selector: 'app-support-tickets-list',
  templateUrl: './support-tickets-list.component.html',
  styleUrls: ['./support-tickets-list.component.css']
})
export class SupportTicketsListComponent implements OnInit, OnDestroy {
  SupportTicketStatus = SupportTicketStatus;
  event: Event;
  supportTickets: SupportTicket[] = [];
  timer: any;

  groups = [
    {
      status: SupportTicketStatus.New,
      header: 'Offen'
    },
    {
      status: SupportTicketStatus.InProgress,
      header: 'In Bearbeitung'
    },
    {
      status: SupportTicketStatus.Closed,
      header: 'Erledigt'
    }
  ];

  constructor(
    private session: SessionService,
    private router: Router,
    private apiClient: EventManagementApiClient
  ) {}

  ngOnInit() {
    this.loadData();
  }

  ngOnDestroy(): void {
    if (this.timer) {
      clearTimeout(this.timer);
    }
  }

  async loadData() {
    if (!this.event) {
      this.event = await this.session.getCurrentEvent();
    }
    this.supportTickets = await this.apiClient
        .supportTickets_List(this.event.id, undefined)
        .toPromise();
    this.timer = setTimeout(() => this.loadData(), REFRESH_INTERVAL);
  }

  filterByStatus(status: SupportTicketStatus) : SupportTicket[] {
    let tickets = this.supportTickets.filter(t => t.status == status);
    if (status === SupportTicketStatus.Closed) {
      return orderBy(tickets, 'closedAt', 'desc');
    }
    return tickets;
  }

  async edit(supportTicket: SupportTicket) {
    await this.setStatus(supportTicket, SupportTicketStatus.InProgress);
    this.router.navigate(["/tickets", supportTicket.ticketId, "true"]);
  }

  async close(supportTicket: SupportTicket) {
    await this.setStatus(supportTicket, SupportTicketStatus.Closed);
    await this.loadData();
  }

  async setStatus(supportTicket: SupportTicket, newStatus: SupportTicketStatus) {
    var args = new SetStatusCommandArgs();
    args.newStatus = newStatus;
    await this.apiClient.supportTickets_SetStatus(supportTicket.id, args).toPromise();
  }
}
