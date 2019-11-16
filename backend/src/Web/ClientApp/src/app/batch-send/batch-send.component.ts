import { Component, OnInit } from '@angular/core';
import { SessionService } from '../services/session.service';
import { EventManagementApiClient, Event, TicketType, BatchSendResult, PaymentStatus } from '../services/event-management-api.client';
import { PageAlertService } from '../services/page-alert.service';

@Component({
  selector: 'app-batch-send',
  templateUrl: './batch-send.component.html',
  styleUrls: ['./batch-send.component.css']
})
export class BatchSendComponent implements OnInit {
  currentEvent: Event;
  sendAll: boolean = false;
  dryRun: boolean;
  paymentStatus = [
    { name: "Offen", value: PaymentStatus.Open },
    { name: "Teilweise bezahlt", value: PaymentStatus.PaidPartial },
    { name: "Bezahlt", value: PaymentStatus.Paid }
  ];
  selectedPaymentStatus: PaymentStatus[] = [];
  ticketTypes: TicketType[] = [];
  selectedTicketTypes: string[] = [];
  result: BatchSendResult;
  

  constructor(
    private session: SessionService,
    private apiClient : EventManagementApiClient,
    private alertService: PageAlertService
  ) {}

  async ngOnInit() {
    this.currentEvent = await this.session.getCurrentEvent();
    this.ticketTypes = await this.apiClient.ticketTypes_GetTicketTypes(this.currentEvent.id).toPromise();
  }

  async submit(dryRun: boolean) {
    this.result = <BatchSendResult>await this.apiClient
      .ticketDelivery_SendBatchMails(
        this.currentEvent.id,
        this.sendAll,
        this.selectedTicketTypes,
        this.selectedPaymentStatus,
        dryRun).toPromise();
    if (!dryRun) {
      this.alertService.showAlert({
        message: "E-Mails wurden zur Warteschlange hinzugefügt.",
        type: "success"
      });
      this.result = null;
    }
  }

  cancel() {
    this.result = null;
  }
}
