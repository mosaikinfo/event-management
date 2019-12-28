import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Ticket, Event, PaymentStatus, EventManagementApiClient, TicketType } from '../services/event-management-api.client';
import { SessionService } from '../services/session.service';
import { PageAlertService } from '../services/page-alert.service';

@Component({
  selector: 'app-ticket-edit',
  templateUrl: './ticket-edit.component.html',
  styleUrls: ['./ticket-edit.component.css']
})
export class TicketEditComponent implements OnInit {
  model : Ticket = new Ticket();
  ticketTypes: TicketType[] = [];
  PaymentStatus = PaymentStatus;
  birthDateMax = new Date();
  isEntranceControl: boolean;

  constructor(
    private session: SessionService,
    private apiClient : EventManagementApiClient,
    private route: ActivatedRoute,
    private router: Router,
    private alertService: PageAlertService,
    @Inject('BASE_URL') public baseUrl: string,
  ) {
    this.model.paymentStatus = PaymentStatus.Open;
    this.isEntranceControl = 
      this.route.snapshot.paramMap
        .get('entranceControl') === "true";
  }

  async ngOnInit() {
    let event = <Event>await this.session.getCurrentEvent();
    this.model.eventId = event.id;
    this.apiClient.ticketTypes_GetTicketTypes(event.id)
      .subscribe((items: TicketType[]) => this.ticketTypes = items);
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadTicket(id);
    }
  }

  async loadTicket(id: string): Promise<void> {
    let ticket = <Ticket>await this.apiClient
      .tickets_GetById(id).toPromise();
    this.model = ticket;
  }

  isNew(): boolean {
    return !this.model.id;
  }
  
  async saveChanges(): Promise<void> {
    if (this.isNew()) {
      let ticket = <Ticket>await this.apiClient
        .tickets_CreateTicket(this.model)
        .toPromise();
      this.alertService.showSaveSuccessAlert()
      this.router.navigate(['/tickets', ticket.id, 'false']);
    } else {
      await this.apiClient
        .tickets_UpdateTicket(this.model.id, this.model)
        .toPromise();
      this.alertService.showSaveSuccessAlert();
      if (this.isEntranceControl) {
        this.goBack();
      }
    }
  }

  async sendMail() {
    let yes = confirm("Bist du sicher, dass du das Ticket per E-Mail versenden willst?");
    if (yes) {
      await this.apiClient
        .ticketDelivery_SendMail(this.model.id)
        .toPromise();
      this.alertService.showAlert({
        message: `Die E-Mail wurde zur Warteschlange hinzugefügt.`,
        type: "success"
      });
    }
  }

  async delete() {
    let yes = confirm(`Bist du sicher, dass du das Ticket ${this.model.ticketNumber} löschen willst?`);
    if (yes) {
      await this.apiClient
        .tickets_DeleteTicket(this.model.id)
        .toPromise();
      this.alertService.showAlert({
        message: `Ticket ${this.model.ticketNumber} wurde gelöscht.`,
        type: "success"
      });
      this.goBack();
    }
  }

  goBack() {
    if (this.isEntranceControl) {
      this.router.navigate(['/entrance-control']);
    } else {
      this.router.navigate(['/tickets']);
    }
  }
}
