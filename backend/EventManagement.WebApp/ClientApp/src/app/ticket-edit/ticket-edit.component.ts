import { Component, OnInit } from '@angular/core';
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
    private alertService: PageAlertService
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
      this.apiClient.tickets_GetById(id)
        .subscribe((ticket: Ticket) => this.model = ticket);
    }
  }

  isNew(): boolean {
    return !this.model.id;
  }
  
  saveChanges() {
    if (this.isNew()) {
      this.apiClient.tickets_CreateTicket(this.model)
        .subscribe((ticket: Ticket) => {
          this.alertService.showSaveSuccessAlert()
          this.router.navigate(['/tickets', ticket.id]);
        });
    } else {
      this.apiClient.tickets_UpdateTicket(this.model.id, this.model)
        .subscribe(() => this.alertService.showSaveSuccessAlert());
    }
  }
}
