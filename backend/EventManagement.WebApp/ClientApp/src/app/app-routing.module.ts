import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuardService } from './services/auth-guard.service';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { EventListComponent } from './event-list/event-list.component';
import { EventFormComponent } from './event-form/event-form.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { EventSettingsComponent } from './event-settings/event-settings.component';

const routes: Routes = [
    { 
        path: '', 
        component: DashboardComponent, 
        pathMatch: 'full', 
        canActivate: [AuthGuardService] 
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
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }