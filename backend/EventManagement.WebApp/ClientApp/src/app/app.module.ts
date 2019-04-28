import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, LOCALE_ID } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CalendarModule, Calendar } from 'primeng/calendar';
import { AppRoutingModule } from './app-routing.module';

import { AuthService } from './services/auth.service';
import { SessionService } from './services/session.service';
import { AlertService } from './services/alert.service';
import { EventManagementApiClient } from './services/event-management-api.client';
import { HttpErrorInterceptor } from './services/http-error.interceptor';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { AuthGuardService } from './services/auth-guard.service';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { EventsListComponent } from './events-list/events-list.component';
import { EventFormComponent } from './event-form/event-form.component';

import { registerLocaleData } from '@angular/common';
import localeDe from '@angular/common/locales/de';
import localeDeExtra from '@angular/common/locales/extra/de';

registerLocaleData(localeDe, 'de-DE', localeDeExtra);

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    AuthCallbackComponent,
    EventsListComponent,
    EventFormComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
    CalendarModule,
    AppRoutingModule
  ],
  providers: [
    {provide: LOCALE_ID, useValue: 'de-DE' },
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
    AuthService,
    AuthGuardService,
    AlertService,
    EventManagementApiClient,
    SessionService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
