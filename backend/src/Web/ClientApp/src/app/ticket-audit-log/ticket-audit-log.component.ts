import { Component, OnInit } from '@angular/core';
import { EventManagementApiClient, AuditEvent } from '../services/event-management-api.client';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ticket-audit-log',
  templateUrl: './ticket-audit-log.component.html',
  styleUrls: ['./ticket-audit-log.component.css']
})
export class TicketAuditLogComponent implements OnInit {
  entries: AuditEvent[] = [];

  constructor(
    private apiClient : EventManagementApiClient,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.loadLogEntries();
  }

  async loadLogEntries(): Promise<void> {
    const ticketId = this.route.snapshot.paramMap.get('id');
    this.entries = <AuditEvent[]>await this.apiClient
      .auditEvents_List(ticketId).toPromise();
  }
}
