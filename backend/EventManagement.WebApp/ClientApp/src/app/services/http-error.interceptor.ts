import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { PageAlertService } from "../page-alert/page-alert.service";

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

    constructor(private alertService: PageAlertService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req)
            .pipe(
                catchError((error: HttpErrorResponse) => {
                    if (error.error instanceof ErrorEvent) {
                        // A client-side or network error occurred. Handle it accordingly.
                        this.showError('HTTP error: ' + error.error.message);
                    } else {
                        // The backend returned an unsuccessful response code.
                        // The response body may contain clues as to what went wrong,
                        this.showError(error.message);
                    }
                    // return an observable with a user-facing error message
                    return throwError(error);
                })
            );
    }

    private showError(error: string) {
        console.error(error);
        this.alertService.showError(error);
    }
}