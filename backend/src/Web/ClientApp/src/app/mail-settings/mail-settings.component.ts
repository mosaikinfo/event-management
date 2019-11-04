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
  needsAuthentication: Boolean;
  demoEmailRecipient: string;

  constructor(
    private apiClient : EventManagementApiClient,
    private route: ActivatedRoute,
    private alertService: PageAlertService
  ) {}

  ngOnInit() {
    this.eventId = this.route.snapshot.paramMap.get('id');
    this.apiClient.mailSettings_GetMailSettings(this.eventId)
        .subscribe((item: MailSettings) => {
          this.model = item;
          this.needsAuthentication = Boolean(this.model.smtpUsername);
        });
  }

  submit() {
    this.apiClient.mailSettings_UpdateMailSettings(this.eventId, this.model)
        .subscribe(() => this.alertService.showSaveSuccessAlert());
  }

  useStartTlsChanged(e) {
    const defaultSmtpPort = 25;
    const defaultTlsPort = 587;
    this.model.smtpPort = e.checked ? defaultTlsPort : defaultSmtpPort;
  }

  needsAuthenticationChanged(e) {
    if (!e.checked) {
      this.model.smtpUsername = null;
      this.model.smtpPassword = null;
    }
  }

  curlyBraces(value: string) {
    return '{{' + value + '}}';
  }

  addDemoEmailRecipient() {
    if (this.demoEmailRecipient) {
      this.model.demoEmailRecipients.push(this.demoEmailRecipient);
      this.demoEmailRecipient = null;
    }
  }

  removeDemoMailRecipient(index: number) {
    this.model.demoEmailRecipients.splice(index, 1);
  }
}
