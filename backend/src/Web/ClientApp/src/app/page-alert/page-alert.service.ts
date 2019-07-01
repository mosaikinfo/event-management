import { Injectable } from '@angular/core';
import { Subject }    from 'rxjs';
 
@Injectable()
export class PageAlertService {
    private alertCreatedSource = new Subject<Alert>();
    alertCreated$ = this.alertCreatedSource.asObservable();
   
    showAlert(alert: Alert) {
      this.alertCreatedSource.next(alert);
    }

    showError(error: string) {
      this.showAlert({
        message: error,
        type: "danger"
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
   * Bootstrap provides styles for the following types: `'success'`, `'info'`, `'warning'`, `'danger'`, `'primary'`,
   * `'secondary'`, `'light'` and `'dark'`.
   */
  type: string;
  message: string;
}