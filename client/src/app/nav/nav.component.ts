import { Component } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent {
  model: any = {};

  constructor(public accountService: AccountService, private router: Router) {}

  ngOnInit() {}

  login() {
    this.accountService.login(this.model).subscribe({
      next: (user) => this.router.navigateByUrl('/members'),
    });
  }

  logout() {
    this.accountService.logout();
  }
}
