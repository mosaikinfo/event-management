import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProgressBarService {

  private count = 0;
  progressBarVisible: boolean;

  show() {
    this.count++;
    setTimeout(() => this.updateVisibility(), 100);
  }

  hide() {
    this.count--;
    this.updateVisibility();
  }

  private updateVisibility() {
    this.progressBarVisible = this.count > 0;
  }
}
