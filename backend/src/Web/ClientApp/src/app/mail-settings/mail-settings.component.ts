import { Component, OnInit } from '@angular/core';
import { EventManagementApiClient, MailSettings } from '../services/event-management-api.client';
import { ActivatedRoute } from '@angular/router';
import { PageAlertService } from '../services/page-alert.service';

@Component({
  selector: 'app-mail-settings',
  templateUrl: './mail-settings.component.html',
  styleUrls: ['./mail-settings.component.css']
})
export class MailSettingsComponent implements OnInit {

  eventId: string;
  model : MailSettings = new MailSettings();

  constructor(
    private apiClient : EventManagementApiClient,
    private route: ActivatedRoute,
    private alertService: PageAlertService
  ) {}

  ngOnInit() {
    this.eventId = this.route.snapshot.paramMap.get('id');
    this.apiClient.mailSettings_GetMailSettings(this.eventId)
        .subscribe((item: MailSettings) => this.model = item);
  }

  submit() {
    this.apiClient.mailSettings_UpdateMailSettings(this.eventId, this.model)
        .subscribe(() => this.alertService.showSaveSuccessAlert());
  }
}
