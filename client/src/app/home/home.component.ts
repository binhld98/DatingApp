import { Component } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {
  registerMode = false;

  constructor() {}

  ngOnInit() {}

  onRegisterMode() {
    this.registerMode = true;
  }

  offRegisterMode() {
    this.registerMode = false;
  }
}
