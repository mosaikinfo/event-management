import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PageAlertService } from '../page-alert/page-alert.service';
import { Ticket, EventManagementApiClient } from '../services/event-management-api.client';

@Component({
  selector: 'app-ticket-edit',
  templateUrl: './ticket-edit.component.html',
  styleUrls: ['./ticket-edit.component.css']
})
export class TicketEditComponent implements OnInit {
  model : Ticket = new Ticket();

  constructor(
    private apiClient : EventManagementApiClient,
    private route: ActivatedRoute,
    private router: Router,
    private alertService: PageAlertService
  ) {}

  ngOnInit() {
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
          this.router.navigate(['..', ticket.id]);
        });
    } else {
      this.apiClient.tickets_UpdateTicket(this.model.id, this.model)
        .subscribe(() => this.alertService.showSaveSuccessAlert());
    }
  }
}
