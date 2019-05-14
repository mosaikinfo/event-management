import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from './services/auth-guard.service';
import { EventGuardService } from './services/event-guard.service';

import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { EventListComponent } from './event-list/event-list.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { EventSettingsComponent } from './event-settings/event-settings.component';
import { TicketListComponent } from './ticket-list/ticket-list.component';
import { TicketEditComponent } from './ticket-edit/ticket-edit.component';
import { EntranceControlComponent } from './entrance-control/entrance-control.component';

const routes: Routes = [
    { 
        path: '', 
        component: DashboardComponent, 
        pathMatch: 'full', 
        canActivate: [AuthGuardService, EventGuardService] 
    },
    {
        path: 'auth-callback',
        component: AuthCallbackComponent
    },
    { 
        path: 'events', 
        component: EventListComponent, 
        canActivate: [AuthGuardService] 
    },
    { 
        path: 'events/new', 
        component: EventSettingsComponent, 
        canActivate: [AuthGuardService] 
    },
    {
        path: 'events/:id',
        component: EventSettingsComponent, 
        canActivate: [AuthGuardService] 
    },
    { 
        path: 'tickets', 
        component: TicketListComponent, 
        canActivate: [AuthGuardService, EventGuardService] 
    },
    { 
        path: 'tickets/new', 
        component: TicketEditComponent, 
        canActivate: [AuthGuardService, EventGuardService] 
    },
    { 
        path: 'tickets/:id', 
        component: TicketEditComponent, 
        canActivate: [AuthGuardService, EventGuardService] 
    },
    { 
        path: 'entrance-control', 
        component: EntranceControlComponent, 
        canActivate: [AuthGuardService, EventGuardService] 
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }