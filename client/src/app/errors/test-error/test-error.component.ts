import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css'],
})
export class TestErrorComponent {
  baseUrl = 'https://localhost:5001/api/';
  validationErrors: string[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {}

  getUnauthorizedError() {
    this.http.get(this.baseUrl + 'buggy/unauthorized').subscribe({
      error: (error) => console.log(error),
    });
  }

  getForbiddenError() {
    this.http.get(this.baseUrl + 'buggy/forbidden').subscribe({
      error: (error) => console.log(error),
    });
  }

  getNotFoundError() {
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe({
      error: (error) => console.log(error),
    });
  }

  getBadRequestError() {
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe({
      error: (error) => console.log(error),
    });
  }

  getServerError() {
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe({
      error: (error) => console.log(error),
    });
  }

  getValidationError() {
    this.http.post(this.baseUrl + 'account/register', {}).subscribe({
      error: (error) => {
        console.log(error);
        this.validationErrors = error;
      },
    });
  }
}
