import { Injectable, Output, EventEmitter } from '@angular/core';
import { Event } from './event-management-api.client';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  private _currentEvent: Event;
  private _currentEventKey = "CurrentEvent";
  
  @Output() onCurrentEventChanged = new EventEmitter<any>();

  constructor() {
    this.getCurrentEvent().then((evt: Event) => {
      // Trigger the event on application start.
      this.onCurrentEventChanged.emit(evt);
    });
  }

  public getCurrentEvent(): Promise<Event> {
    if (!this._currentEvent) {
      const json = localStorage.getItem(this._currentEventKey);
      let result = JSON.parse(json) as Event;
      this._currentEvent = result;
    }
    return Promise.resolve(this._currentEvent);
  }

  public setCurrentEvent(evt: Event) {
    this._currentEvent = evt;
    const json = JSON.stringify(evt);
    localStorage.setItem(this._currentEventKey, json);
    this.onCurrentEventChanged.emit(evt);
  }

  public unsetCurrentEvent() {
    localStorage.removeItem(this._currentEventKey);
    this.onCurrentEventChanged.emit(null);
  }
}
