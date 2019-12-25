import { Component, OnInit } from '@angular/core';
import { SessionService } from '../services/session.service';
import { EventManagementApiClient, Event, TicketQuotaReportRow } from '../services/event-management-api.client';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit {
  event: Event;
  quotas: TicketQuotaReportRow[] = [];

  constructor(
    private session: SessionService,
    private apiClient: EventManagementApiClient,
  ) {}

  async ngOnInit() {
    this.event = await this.session.getCurrentEvent();
    if (this.event) {
      this.loadQuotas();
    }
  }

  async loadQuotas(): Promise<void> {
    this.quotas = await this.apiClient
      .ticketQuotaReport_GetReport(this.event.id)
      .toPromise();
  }
}
