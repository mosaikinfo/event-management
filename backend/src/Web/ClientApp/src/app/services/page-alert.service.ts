import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/components/common/api';
 
@Injectable({ providedIn: 'root'})
export class PageAlertService {
    private _severity = {
      'warning': 'warn',
      'danger': 'error'
    }

    constructor(
      private messageService: MessageService) {}
   
    showAlert(alert: Alert) {
      this.messageService.add({
        severity: this._severity[alert.type] || alert.type,
        summary: alert.title,
        detail: alert.message,
        sticky: alert.sticky
      });
    }

    showError(error: string, sticky: boolean = false) {
      this.showAlert({
        message: error,
        type: "danger",
        sticky: sticky
      });
    }

    showSaveSuccessAlert() {
      this.showAlert({
        message: "Änderungen gespeichert!",
        type: "success"
      });
    }

    showNotImplemented() {
      this.showAlert({
        message: "Diese Funktion ist noch nicht umgesetzt.",
        type: "warning"
      });
    }
}

export interface Alert {
  /**
   * Type of the alert.
   *
   * `'success'`, `'info'`, `'warning'`, `'danger'`
   */
  type: string;
  title?: string;
  message: string;
  sticky?: boolean;
}