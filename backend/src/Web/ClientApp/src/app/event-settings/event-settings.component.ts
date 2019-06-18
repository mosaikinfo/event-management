import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-event-settings',
  templateUrl: './event-settings.component.html',
  styleUrls: ['./event-settings.component.css']
})
export class EventSettingsComponent implements OnInit {
  eventId: number;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.eventId = +this.route.snapshot.paramMap.get('id');
  }

}
