import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageAlertService } from '../page-alert/page-alert.service';
import { TicketType, EventManagementApiClient } from '../services/event-management-api.client';

@Component({
  selector: 'app-ticket-types-edit',
  templateUrl: './ticket-types-edit.component.html',
  styleUrls: ['./ticket-types-edit.component.css']
})
export class TicketTypesEditComponent implements OnInit {

  eventId: number;
  ticketTypes: TicketType[] = [];

  constructor(
    private apiClient : EventManagementApiClient,
    private route: ActivatedRoute,
    private alertService: PageAlertService
  ) {}

  ngOnInit() {
    this.eventId = +this.route.snapshot.paramMap.get('id');
    if (this.eventId) {
      this.apiClient.ticketTypes_GetTicketTypes(this.eventId)
        .subscribe((items: TicketType[]) => this.ticketTypes = items);
    }
  }

  add() {
    this.ticketTypes.push(new TicketType());
  }

  remove(ticketType: TicketType) {
    this.ticketTypes.splice(this.ticketTypes.indexOf(ticketType), 1);
  }

  saveChanges() {
    this.apiClient.ticketTypes_AddOrUpdateTicketTypes(this.eventId, this.ticketTypes)
      .subscribe((items: TicketType[]) => {
        this.ticketTypes = items;
        this.alertService.showSaveSuccessAlert();
      });
  }
}
