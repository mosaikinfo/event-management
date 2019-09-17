import { Component, OnInit } from '@angular/core';
import { SessionService } from '../services/session.service';
import { EventManagementApiClient, TicketQuotaReportRow } from '../services/event-management-api.client';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit {
  quotas: TicketQuotaReportRow[] = [];

  constructor(
    private session: SessionService,
    private apiClient: EventManagementApiClient
  ) {}

  async ngOnInit() {
    let event = await this.session.getCurrentEvent();
    this.quotas = await this.apiClient
      .ticketQuotaReport_GetReport(event.id)
      .toPromise();
  }
}
