import { Component } from '@angular/core';
import { ProgressBarService } from './services/progressbar.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(
    public progressBarService: ProgressBarService) {}
}
