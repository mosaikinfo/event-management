import { Component, OnInit } from '@angular/core';
import { EventManagementApiClient, Event } from '../services/event-management-api.client';
import { Router } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.css']
})
export class EventFormComponent implements OnInit {

  model : Event = new Event();
  today: NgbDateStruct = this.getToday()

  constructor(
    private apiClient : EventManagementApiClient,
    private router: Router 
  ) {}

  ngOnInit() {
  }

  submit() {
    this.apiClient.events_CreateEvent(this.model)
      .subscribe(() => this.router.navigate(['/events']));
  }

  private getToday(): NgbDateStruct {
    var date = new Date();
    return {
      day: date.getDate(),
      month: date.getMonth() + 1,
      year: date.getFullYear()
    }
  }
}
