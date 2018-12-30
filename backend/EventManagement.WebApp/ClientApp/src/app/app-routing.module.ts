import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuardService } from './services/auth-guard.service';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

const routes: Routes = [
    { 
        path: '', 
        component: HomeComponent, 
        pathMatch: 'full', 
        canActivate: [AuthGuardService] 
    },
    {
        path: 'auth-callback',
        component: AuthCallbackComponent
    },
    { 
        path: 'counter', 
        component: CounterComponent, 
        canActivate: [AuthGuardService] 
    },
    { 
        path: 'fetch-data', 
        component: FetchDataComponent, 
        canActivate: [AuthGuardService] 
    },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }