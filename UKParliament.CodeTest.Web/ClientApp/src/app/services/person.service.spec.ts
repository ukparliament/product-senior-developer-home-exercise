import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from "@angular/core/testing";
import { PersonViewModel } from "../models/person-view-model";
import { PersonService } from "./person.service";

describe('PersonService', () => {
  let service: PersonService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PersonService]
    });
    service = TestBed.inject(PersonService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should retrieve all people', () => {
    const dummyPeople: PersonViewModel[] = [
      { id: 1, firstName: 'John', lastName: 'Doe', dateOfBirth: '1990-01-01', departmentId: 1, departmentName: 'HR', email: 'john@example.com' }
    ];

    service.getAll().subscribe(people => {
      expect(people.length).toBe(1);
      expect(people).toEqual(dummyPeople);
    });

    const req = httpMock.expectOne('/api/person');
    expect(req.request.method).toBe('GET');
    req.flush(dummyPeople);
  });
});
