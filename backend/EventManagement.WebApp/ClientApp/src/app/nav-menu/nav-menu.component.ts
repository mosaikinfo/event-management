import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  isExpanded = false;
  currentUser: string = "";

  constructor(private authService: AuthService) {
    this.authService.onUserLoggedIn
      .subscribe(claims => this.currentUser = claims['name']);
  }

  logout() {
    this.authService.startLogout();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
