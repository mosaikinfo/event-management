import { Injectable } from "@angular/core";

@Injectable()
export class AlertService {

    public showError(message: string): void {
        console.error(message);
        alert(`Error: ${message}`);
    }
}