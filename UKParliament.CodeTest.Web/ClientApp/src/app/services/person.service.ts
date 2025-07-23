import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { PersonViewModel } from '../models/person-view-model';
import { ValidationError } from '../models/validation-error';

@Injectable({
  providedIn: 'root'
})

export class PersonService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  private readonly personApiUrl = this.baseUrl + 'api/person';

  getAll(): Observable<PersonViewModel[]> {
    return this.http.get<PersonViewModel[]>(this.personApiUrl ).pipe(
      catchError(this.handleError)
    );
  }

  getById(id: number): Observable<PersonViewModel> {
    return this.http.get<PersonViewModel>(`${this.personApiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  add(person: PersonViewModel): Observable<void> {
    return this.http.post<void>(this.personApiUrl, person).pipe(
      catchError(this.handleError)
    );
  }

  update(id: number, person: PersonViewModel): Observable<void> {
    return this.http.put<void>(`${this.personApiUrl}/${id}`, person).pipe(
      catchError(this.handleError)
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.personApiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    if (error.status === 400) {
      return throwError(() => error.error as ValidationError);
    } else if (error.status === 404) {
      return throwError(() => ({ errors: ['Person not found'] } as ValidationError));
    }
    return throwError(() => ({ errors: ['An unexpected error occurred'] } as ValidationError));
  }
}
