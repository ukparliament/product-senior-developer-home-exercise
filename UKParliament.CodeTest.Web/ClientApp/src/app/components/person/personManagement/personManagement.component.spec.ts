import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PersonManagementComponent } from './personManagement.component';
import { PersonService } from '../../../services/person.service';
import { DepartmentService } from '../../../services/department.service';
import { of, throwError } from 'rxjs';
import { PersonViewModel } from '../../../models/person-view-model';
import { Department } from '../../../models/department-view-model';
import { ValidationError } from '../../../models/validation-error';
import { FormsModule } from '@angular/forms';
import { PersonListComponent } from '../personlist/person-list.component';

describe('PersonManagementComponent', () => {
  let component: PersonManagementComponent;
  let fixture: ComponentFixture<PersonManagementComponent>;
  let mockPersonService: jasmine.SpyObj<PersonService>;
  let mockDepartmentService: jasmine.SpyObj<DepartmentService>;

  const testPerson: PersonViewModel = {
    id: 1,
    firstName: 'John',
    lastName: 'Doe',
    dateOfBirth: '1990-01-01',
    departmentId: 1,
    departmentName: 'IT',
    email: 'john@example.com'
  };

  const testDepartment: Department = {
    id: 1,
    name: 'IT'
  };

  beforeEach(async () => {
    mockPersonService = jasmine.createSpyObj('PersonService', [
      'getAll', 'add', 'update', 'delete'
    ]);
    mockDepartmentService = jasmine.createSpyObj('DepartmentService', ['getAll']);

    await TestBed.configureTestingModule({
      declarations: [PersonManagementComponent, PersonListComponent],
      imports: [FormsModule],
      providers: [
        { provide: PersonService, useValue: mockPersonService },
        { provide: DepartmentService, useValue: mockDepartmentService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PersonManagementComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('should load people and departments', () => {
      mockPersonService.getAll.and.returnValue(of([testPerson]));
      mockDepartmentService.getAll.and.returnValue(of([testDepartment]));

      fixture.detectChanges();

      expect(mockPersonService.getAll).toHaveBeenCalled();
      expect(mockDepartmentService.getAll).toHaveBeenCalled();
      expect(component.people).toEqual([testPerson]);
      expect(component.departments).toEqual([testDepartment]);
    });

    it('should handle errors when loading people', () => {
      const errorResponse: ValidationError = { errors: ['Load failed'] };
      mockPersonService.getAll.and.returnValue(throwError(() => errorResponse));
      mockDepartmentService.getAll.and.returnValue(of([]));

      fixture.detectChanges();

      expect(component.errors).toEqual(errorResponse.errors);
    });

    it('should handle errors when loading departments', () => {
      const errorResponse: ValidationError = { errors: ['Load failed'] };
      mockPersonService.getAll.and.returnValue(of([]));
      mockDepartmentService.getAll.and.returnValue(throwError(() => errorResponse));

      fixture.detectChanges();

      expect(component.errors).toEqual(errorResponse.errors);
    });
  });

  describe('selectPerson', () => {
    it('should select a person and clear errors', () => {
      component.errors = ['Previous error'];
      component.selectPerson(testPerson);

      expect(component.selectedPerson).toEqual({ ...testPerson });
      expect(component.errors).toEqual([]);
    });
  });

  describe('addNewPerson', () => {
    it('should initialize a new person and clear errors', () => {
      component.errors = ['Previous error'];
      component.addNewPerson();

      expect(component.selectedPerson).toEqual({
        id: 0,
        firstName: '',
        lastName: '',
        dateOfBirth: '',
        departmentId: 0,
        departmentName: '',
        email: ''
      });
      expect(component.errors).toEqual([]);
    });
  });

  describe('savePerson', () => {
    beforeEach(() => {
      mockPersonService.getAll.and.returnValue(of([]));
      mockDepartmentService.getAll.and.returnValue(of([]));
      fixture.detectChanges();
    });

    it('should add a new person when id is 0', () => {
      const newPerson = { ...testPerson, id: 0 };
      mockPersonService.add.and.returnValue(of(void 0));

      component.savePerson(newPerson);

      expect(mockPersonService.add).toHaveBeenCalledWith(newPerson);
      expect(mockPersonService.getAll).toHaveBeenCalledTimes(2);
      expect(component.selectedPerson).toBeNull();
      expect(component.errors).toEqual([]);
    });

    it('should update an existing person', () => {
      mockPersonService.update.and.returnValue(of(void 0));

      component.savePerson(testPerson);

      expect(mockPersonService.update).toHaveBeenCalledWith(testPerson.id, testPerson);
      expect(mockPersonService.getAll).toHaveBeenCalledTimes(2);
      expect(component.selectedPerson).toBeNull();
      expect(component.errors).toEqual([]);
    });

    it('should handle errors when saving', () => {
      const errorResponse = ['Validation failed'];
      mockPersonService.add.and.returnValue(throwError(() => errorResponse));

      component.savePerson({ ...testPerson, id: 0 });

      expect(component.errors).toEqual(errorResponse);
    });
  });

  describe('deletePerson', () => {
    beforeEach(() => {
      spyOn(window, 'confirm').and.returnValue(true);
      mockPersonService.getAll.and.returnValue(of([]));
      mockDepartmentService.getAll.and.returnValue(of([]));
      mockPersonService.delete.and.returnValue(of(void 0));
      fixture.detectChanges();
    });

    it('should delete a person when confirmed', () => {
      component.deletePerson(1);

      expect(window.confirm).toHaveBeenCalledWith('Are you sure you want to delete this person?');
      expect(mockPersonService.delete).toHaveBeenCalledWith(1);
      expect(mockPersonService.getAll).toHaveBeenCalledTimes(2);
      expect(component.selectedPerson).toBeNull();
      expect(component.errors).toEqual([]);
    });

    it('should not delete if not confirmed', () => {
      (window.confirm as jasmine.Spy).and.returnValue(false);

      component.deletePerson(1);

      expect(mockPersonService.delete).not.toHaveBeenCalled();
    });

    it('should handle errors when deleting', () => {
      const errorResponse = ['Delete failed'];
      mockPersonService.delete.and.returnValue(throwError(() => errorResponse));

      component.deletePerson(1);

      expect(component.errors).toEqual(errorResponse);
    });
  });
});
