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
}

export interface Alert {
    type: string;
    message: string;
}