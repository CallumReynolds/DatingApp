import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  // Stores username and password
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  // Method for logging
  login() {
    // always need to subscribe to observables
    this.authService.login(this.model).subscribe(next => {
      console.log('Logged in!');
    }, error => {
      console.log('Failed!');
    });
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token;
  }

  logout() {
    localStorage.removeItem('token');
    console.log('logged out');
  }

}
