import { Component } from '@angular/core';
import { Alert, PageAlertService } from './page-alert.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-page-alert',
  templateUrl: './page-alert.component.html',
  styleUrls: ['./page-alert.component.css']
})
export class PageAlertComponent {
  alerts: Alert[] = []

  constructor(alertService: PageAlertService) {
    alertService.alertCreated$
      .subscribe(alert => {
        this.alerts.push(alert);
        setTimeout(() => this.close(alert), 3000);
      });
  }

  close(alert: Alert) {
    this.alerts.splice(this.alerts.indexOf(alert), 1);
  }
}