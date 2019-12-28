import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, LOCALE_ID } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CalendarModule } from 'primeng/calendar';
import { TableModule } from 'primeng/table';
import { InputSwitchModule } from 'primeng/inputswitch';
import { TriStateCheckboxModule } from 'primeng/tristatecheckbox';
import { ProgressBarModule } from 'primeng/progressbar';
import { ToastModule } from 'primeng/toast';
import { DropdownModule } from 'primeng/dropdown';
import { RadioButtonModule } from 'primeng/radiobutton';
import { CheckboxModule } from 'primeng/checkbox';
import { BlockUIModule } from 'primeng/blockui';
import { PanelModule } from 'primeng/panel';
import { FieldsetModule } from 'primeng/fieldset';

import { MomentModule } from 'ngx-moment';
import 'moment/locale/de';

import { AppRoutingModule } from './app-routing.module';
import { AuthService } from './services/auth.service';
import { AuthGuardService } from './services/auth-guard.service';
import { EventGuardService } from './services/event-guard.service';
import { SessionService } from './services/session.service';
import { PageAlertService } from './services/page-alert.service';
import { ProgressBarService } from './services/progressbar.service';
import { EventManagementApiClient, API_BASE_URL } from './services/event-management-api.client';
import { HttpLoaderInterceptor } from './services/http-loader.interceptor';
import { HttpErrorInterceptor } from './services/http-error.interceptor';

import { registerLocaleData } from '@angular/common';
import localeDe from '@angular/common/locales/de';
import localeDeExtra from '@angular/common/locales/extra/de';
import { MessageService } from 'primeng/components/common/api';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { EventListComponent } from './event-list/event-list.component';
import { EventEditComponent } from './event-edit/event-edit.component';
import { EventSettingsComponent } from './event-settings/event-settings.component';
import { ButtonBackComponent } from './button-back/button-back.component';
import { TicketTypesEditComponent } from './ticket-types-edit/ticket-types-edit.component';
import { TicketListComponent } from './ticket-list/ticket-list.component';
import { TicketEditComponent } from './ticket-edit/ticket-edit.component';
import { EntranceControlComponent } from './entrance-control/entrance-control.component';
import { EntranceControlManualComponent } from './entrance-control-manual/entrance-control-manual.component';
import { EntranceControlAppComponent } from './entrance-control-app/entrance-control-app.component';
import { MailSettingsComponent } from './mail-settings/mail-settings.component';
import { TicketDetailComponent } from './ticket-detail/ticket-detail.component';
import { TicketAuditLogComponent } from './ticket-audit-log/ticket-audit-log.component';
import { EventDangerZoneComponent } from './event-danger-zone/event-danger-zone.component';
import { BatchSendComponent } from './batch-send/batch-send.component';
import { LiveStatusComponent } from './live-status/live-status.component';
import { SupportTicketsListComponent } from './support-tickets-list/support-tickets-list.component';

registerLocaleData(localeDe, 'de-DE', localeDeExtra);

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    DashboardComponent,
    AuthCallbackComponent,
    EventListComponent,
    EventEditComponent,
    EventSettingsComponent,
    ButtonBackComponent,
    TicketTypesEditComponent,
    TicketListComponent,
    TicketEditComponent,
    EntranceControlComponent,
    EntranceControlManualComponent,
    EntranceControlAppComponent,
    MailSettingsComponent,
    TicketDetailComponent,
    TicketAuditLogComponent,
    EventDangerZoneComponent,
    BatchSendComponent,
    LiveStatusComponent,
    SupportTicketsListComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
    CalendarModule,
    AppRoutingModule,
    TableModule,
    InputSwitchModule,
    TriStateCheckboxModule,
    ProgressBarModule,
    ToastModule,
    DropdownModule,
    RadioButtonModule,
    CheckboxModule,
    BlockUIModule,
    PanelModule,
    FieldsetModule,
    MomentModule
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'de-DE' },
    { provide: HTTP_INTERCEPTORS, useClass: HttpLoaderInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
    { provide: API_BASE_URL, useExisting: 'BASE_URL'},
    AuthService,
    AuthGuardService,
    EventGuardService,
    EventManagementApiClient,
    SessionService,
    PageAlertService,
    ProgressBarService,
    MessageService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
