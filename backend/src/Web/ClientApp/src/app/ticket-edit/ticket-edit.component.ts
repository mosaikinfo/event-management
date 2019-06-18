import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PageAlertService } from '../page-alert/page-alert.service';
import { Ticket, Event, PaymentStatus, EventManagementApiClient, TicketType } from '../services/event-management-api.client';
import { SessionService } from '../services/session.service';

@Component({
  selector: 'app-ticket-edit',
  templateUrl: './ticket-edit.component.html',
  styleUrls: ['./ticket-edit.component.css']
})
export class TicketEditComponent implements OnInit {
  model : Ticket = new Ticket();
  ticketTypes: TicketType[] = [];
  PaymentStatus = PaymentStatus;

  constructor(
    private session: SessionService,
    private apiClient : EventManagementApiClient,
    private route: ActivatedRoute,
    private router: Router,
    private alertService: PageAlertService,
    @Inject('BASE_URL') public baseUrl: string,
  ) {
    this.model.paymentStatus = PaymentStatus.Open;
  }

  async ngOnInit() {
    let event = <Event>await this.session.getCurrentEvent();
    this.model.eventId = event.id;
    this.apiClient.ticketTypes_GetTicketTypes(event.id)
      .subscribe((items: TicketType[]) => this.ticketTypes = items);
    const id = +this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadTicket(id);
    }
  }

  async loadTicket(id: number): Promise<void> {
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
      this.router.navigate(['/tickets', ticket.id]);
    } else {
      await this.apiClient
        .tickets_UpdateTicket(this.model.id, this.model)
        .toPromise();
      this.alertService.showSaveSuccessAlert();
    }
  }

  async saveAndGoBack() {
    await this.saveChanges();
    this.goBack();
  }

  async delete() {
    let yes = confirm(`Sind Sie sicher, dass Sie Ticket ${this.model.ticketNumber} löschen wollen?`);
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
    this.router.navigate(['/tickets']);
  }
}
