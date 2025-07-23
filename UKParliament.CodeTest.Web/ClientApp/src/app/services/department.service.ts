import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Department } from '../models/department-view-model';

@Injectable({
  providedIn: 'root'
})

export class DepartmentService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  private departmentUrl = this.baseUrl + '/api/department';


  getAll(): Observable<Department[]> {
    return this.http.get<Department[]>(this.departmentUrl).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    return throwError(() => ({ errors: ['Failed to load departments'] } as any));
  }
}
