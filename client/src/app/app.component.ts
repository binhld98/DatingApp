import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'DatingApp';
  users: any;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: (users) => (this.users = users),
      error: (error) => console.log(error),
      complete: () => console.log('request completed'),
    });
  }
}
