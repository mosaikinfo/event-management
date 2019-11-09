import { Component, OnInit, OnDestroy } from '@angular/core';
import { SessionService } from '../services/session.service';
import { EventManagementApiClient, Event, TicketQuotaReportRow, EventStatus } from '../services/event-management-api.client';
import { PageAlertService } from '../services/page-alert.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit, OnDestroy {
  event: Event;
  status: EventStatus;
  quotas: TicketQuotaReportRow[] = [];
  timer: any;
  blink: boolean = false;
  timerAnimation: any;

  constructor(
    private session: SessionService,
    private apiClient: EventManagementApiClient,
    private alertService: PageAlertService
  ) {}

  async ngOnInit() {
    this.event = await this.session.getCurrentEvent();
    if (this.event) {
      this.loadStatus();
      this.loadQuotas();
      this.timerAnimation = setInterval(() => this.blink = !this.blink, 1000);
    }
  }

  async loadStatus(): Promise<void> {
    try {
      this.status = await this.apiClient
        .eventStatus_GetStatus(this.event.id)
        .toPromise();
    } catch {
      this.alertService.showError(
        "Die Verbindung zum Server wurde unterbrochen.", true);
      this.timer = null;
      return;
    }
    this.timer = setTimeout(() => this.loadStatus(), 1000);
  }

  async loadQuotas(): Promise<void> {
    this.quotas = await this.apiClient
      .ticketQuotaReport_GetReport(this.event.id)
      .toPromise();
  }

  ngOnDestroy(): void {
    if (this.timer) {
      clearTimeout(this.timer);
    }
    if (this.timerAnimation) {
      clearInterval(this.timerAnimation);
    }
  }
}
