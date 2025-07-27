import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PersonService } from './person.service';
import { PersonViewModel } from '../models/person-view-model';
import { ValidationError } from '../models/validation-error';

describe('PersonService', () => {
  let service: PersonService;
  let httpMock: HttpTestingController;
  const baseUrl = 'http://localhost:9876/';
  const testPerson: PersonViewModel = {
    id: 1,
    firstName: 'John',
    lastName: 'Doe',
    dateOfBirth: '1990-01-01',
    departmentId: 1,
    departmentName : "HR",
    email: 'john.doe@example.com'
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        PersonService,
        { provide: 'BASE_URL', useValue: baseUrl }
      ]
    });

    service = TestBed.inject(PersonService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('getAll', () => {
    it('should return all persons', () => {
      const testData: PersonViewModel[] = [testPerson];

      service.getAll().subscribe(persons => {
        expect(persons).toEqual(testData);
      });

      const req = httpMock.expectOne(`${baseUrl}api/person`);
      expect(req.request.method).toBe('GET');
      req.flush(testData);
    });

    it('should handle 400 error', () => {
      const errorResponse: ValidationError = { errors: ['Validation failed'] };

      service.getAll().subscribe({
        error: (err: ValidationError) => {
          expect(err).toEqual(errorResponse);
        }
      });

      const req = httpMock.expectOne(`${baseUrl}api/person`);
      req.flush(errorResponse, { status: 400, statusText: 'Bad Request' });
    });

    it('should handle 404 error', () => {
      service.getAll().subscribe({
        error: (err: ValidationError) => {
          expect(err.errors).toContain('Person not found');
        }
      });

      const req = httpMock.expectOne(`${baseUrl}api/person`);
      req.flush(null, { status: 404, statusText: 'Not Found' });
    });

    it('should handle unexpected error', () => {
      service.getAll().subscribe({
        error: (err: ValidationError) => {
          expect(err.errors).toContain('An unexpected error occurred');
        }
      });

      const req = httpMock.expectOne(`${baseUrl}api/person`);
      req.flush(null, { status: 500, statusText: 'Server Error' });
    });
  });

  describe('getById', () => {
    it('should return a specific person', () => {
      service.getById(1).subscribe(person => {
        expect(person).toEqual(testPerson);
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/1`);
      expect(req.request.method).toBe('GET');
      req.flush(testPerson);
    });

    it('should handle errors for getById', () => {
      service.getById(1).subscribe({
        error: (err: ValidationError) => {
          expect(err.errors).toContain('Person not found');
        }
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/1`);
      req.flush(null, { status: 404, statusText: 'Not Found' });
    });
  });

  describe('add', () => {
    it('should add a new person', () => {
      service.add(testPerson).subscribe(response => {
        expect(response).toBeNull();
      });

      const req = httpMock.expectOne(`${baseUrl}api/person`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(testPerson);
      req.flush(null);
    });

    it('should handle validation errors for add', () => {
      const errorResponse: ValidationError = { errors: ['Name is required'] };

      service.add(testPerson).subscribe({
        error: (err: ValidationError) => {
          expect(err).toEqual(errorResponse);
        }
      });

      const req = httpMock.expectOne(`${baseUrl}api/person`);
      req.flush(errorResponse, { status: 400, statusText: 'Bad Request' });
    });
  });

  describe('update', () => {
    it('should update a person', () => {
      service.update(1, testPerson).subscribe(response => {
        expect(response).toBeNull();
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/1`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(testPerson);
      req.flush(null);
    });

    it('should handle ID mismatch', () => {
      const errorResponse: ValidationError = { errors: ['ID mismatch'] };

      service.update(2, { ...testPerson, id: 1 }).subscribe({
        error: (err: ValidationError) => {
          expect(err).toEqual(errorResponse);
        }
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/2`);
      req.flush(errorResponse, { status: 400, statusText: 'Bad Request' });
    });
  });

  describe('delete', () => {
    it('should delete a person', () => {
      service.delete(1).subscribe(response => {
        expect(response).toBeNull();
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/1`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });

    it('should handle not found for delete', () => {
      service.delete(999).subscribe({
        error: (err: ValidationError) => {
          expect(err.errors).toContain('Person not found');
        }
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/999`);
      req.flush(null, { status: 404, statusText: 'Not Found' });
    });
  });
});
