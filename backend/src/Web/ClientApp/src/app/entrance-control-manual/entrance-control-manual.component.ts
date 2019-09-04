import { Component, Inject, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-entrance-control-manual',
  templateUrl: './entrance-control-manual.component.html',
  styleUrls: ['./entrance-control-manual.component.css']
})
export class EntranceControlManualComponent {
  ticketNumber: string;
  @ViewChild("textBox") textBox: ElementRef;

  constructor(
    @Inject('BASE_URL') public baseUrl: string
  ) {}

  validateTicket() {
    const number = encodeURIComponent(this.ticketNumber);
    const url = `${this.baseUrl}/tickets/validate?number=${number}`;
    window.open(url, "_blank");
    this.textBox.nativeElement.select();
  }
}
