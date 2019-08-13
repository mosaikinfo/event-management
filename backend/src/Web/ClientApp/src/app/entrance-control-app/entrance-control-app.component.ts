import { Component, OnInit, Inject } from '@angular/core';
import { SessionService } from '../services/session.service';
import { Event } from '../services/event-management-api.client';

@Component({
  selector: 'app-entrance-control-app',
  templateUrl: './entrance-control-app.component.html',
  styleUrls: ['./entrance-control-app.component.css']
})
export class EntranceControlAppComponent implements OnInit {

  qrCodeImageUrl: string;

  constructor(
    private session: SessionService,
    @Inject('BASE_URL') public baseUrl: string) { }

  ngOnInit() {
    this.session.getCurrentEvent()
      .then((event: Event) => {
        this.qrCodeImageUrl = `${this.baseUrl}/events/${event.id}/masterqrcodes/my.png`
      });
  }

}
