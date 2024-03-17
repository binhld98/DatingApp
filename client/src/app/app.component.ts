import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'DatingApp';
  users: any;

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {}

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userJson = localStorage.getItem('user');
    if (!userJson) {
      return;
    }

    this.accountService.setCurrentUser(JSON.parse(userJson));
  }
}
