import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { PersonEditorComponent } from './person-editor.component';
import { PersonViewModel } from '../../../models/person-view-model';
import { Department } from '../../../models/department-view-model';

describe('PersonEditorComponent', () => {
  let component: PersonEditorComponent;
  let fixture: ComponentFixture<PersonEditorComponent>;
  let formBuilder: FormBuilder;

  const mockPerson: PersonViewModel = {
    id: 1,
    firstName: 'John',
    lastName: 'Doe',
    dateOfBirth: '1990-01-01',
    departmentId: 2,
    departmentName: 'IT',
    email: 'john.doe@example.com'
  };

  const mockDepartments: Department[] = [
    { id: 1, name: 'HR' },
    { id: 2, name: 'IT' },
    { id: 3, name: 'Finance' }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      declarations: [PersonEditorComponent],
      providers: [FormBuilder]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonEditorComponent);
    component = fixture.componentInstance;
    formBuilder = TestBed.inject(FormBuilder);
    component.person = {
      id: 0,
      firstName: '',
      lastName: '',
      dateOfBirth: '',
      departmentId: 0,
      departmentName: '',
      email: ''
    };
    component.departments = mockDepartments;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with empty values when no person input', () => {
    expect(component.personForm.value).toEqual({
      id: 0,
      firstName: '',
      lastName: '',
      dateOfBirth: '',
      departmentId: 0,
      email: ''
    });
  });

  it('should patch form values when person input changes', () => {
    component.person = mockPerson;
    component.ngOnChanges();
    fixture.detectChanges();

    expect(component.personForm.value).toEqual({
      id: mockPerson.id,
      firstName: mockPerson.firstName,
      lastName: mockPerson.lastName,
      dateOfBirth: mockPerson.dateOfBirth,
      departmentId: mockPerson.departmentId,
      email: mockPerson.email
    });
  });

  it('should display correct legend for new person', () => {
    component.person = { ...mockPerson, id: 0 };
    component.ngOnChanges();
    fixture.detectChanges();

    const legend = fixture.nativeElement.querySelector('legend');
    expect(legend.textContent).toContain('Add Person');
  });

  it('should display correct legend for existing person', () => {
    component.person = mockPerson;
    component.ngOnChanges();
    fixture.detectChanges();

    const legend = fixture.nativeElement.querySelector('legend');
    expect(legend.textContent).toContain('Edit Person');
  });

  it('should emit savePerson event with form values when form is valid', () => {
    spyOn(component.savePerson, 'emit');
    component.person = mockPerson;
    component.ngOnChanges();
    fixture.detectChanges();

    component.save();

    expect(component.savePerson.emit).toHaveBeenCalledWith(component.personForm.value);
  });

  it('should not emit savePerson event when form is invalid', () => {
    spyOn(component.savePerson, 'emit');
    component.personForm.patchValue({ firstName: '' }); // Make form invalid
    fixture.detectChanges();

    component.save();

    expect(component.savePerson.emit).not.toHaveBeenCalled();
  });

  it('should disable save button when form is invalid', () => {
    component.personForm.patchValue({ firstName: '' }); // Make form invalid
    fixture.detectChanges();

    const saveButton = fixture.nativeElement.querySelector('button[type="submit"]');
    expect(saveButton.disabled).toBeTrue();
  });

  it('should enable save button when form is valid', () => {
    component.person = mockPerson;
    component.ngOnChanges();
    fixture.detectChanges();

    const saveButton = fixture.nativeElement.querySelector('button[type="submit"]');
    expect(saveButton.disabled).toBeFalse();
  });

  describe('Form Validation', () => {
    it('should require firstName', () => {
      const control = component.personForm.get('firstName');
      control?.setValue('');
      expect(control?.valid).toBeFalse();
      expect(component.getErrorMessage('firstName')).toBe('firstName is required');
    });

    it('should validate firstName max length', () => {
      const control = component.personForm.get('firstName');
      control?.setValue('a'.repeat(51));
      expect(control?.valid).toBeFalse();
      expect(component.getErrorMessage('firstName')).toBe('firstName exceeds maximum length');
    });

    it('should require lastName', () => {
      const control = component.personForm.get('lastName');
      control?.setValue('');
      expect(control?.valid).toBeFalse();
      expect(component.getErrorMessage('lastName')).toBe('lastName is required');
    });

    it('should validate lastName max length', () => {
      const control = component.personForm.get('lastName');
      control?.setValue('a'.repeat(51));
      expect(control?.valid).toBeFalse();
      expect(component.getErrorMessage('lastName')).toBe('lastName exceeds maximum length');
    });

    it('should require dateOfBirth', () => {
      const control = component.personForm.get('dateOfBirth');
      control?.setValue('');
      expect(control?.valid).toBeFalse();
      expect(component.getErrorMessage('dateOfBirth')).toBe('dateOfBirth is required');
    });

    it('should require departmentId to be greater than 0', () => {
      const control = component.personForm.get('departmentId');
      control?.setValue(0);
      expect(control?.valid).toBeFalse();
      expect(component.getErrorMessage('departmentId')).toBe('Please select a department');
    });

    it('should require valid email', () => {
      const control = component.personForm.get('email');
      control?.setValue('invalid-email');
      expect(control?.valid).toBeFalse();
      expect(component.getErrorMessage('email')).toBe('Invalid email format');
    });

    it('should validate email max length', () => {
      const control = component.personForm.get('email');
      control?.setValue(`${'a'.repeat(100)}@example.com`);
      expect(control?.valid).toBeFalse();
      expect(component.getErrorMessage('email')).toBe('email exceeds maximum length');
    });
  });

  describe('Error Message Display', () => {
    it('should show error message when firstName is invalid and touched', () => {
      const control = component.personForm.get('firstName');
      control?.setValue('');
      control?.markAsTouched();
      fixture.detectChanges();

      const errorMessage = fixture.nativeElement.querySelector('#firstName-error');
      expect(errorMessage).toBeTruthy();
      expect(errorMessage.textContent).toContain('firstName is required');
    });

    it('should add error class to input when firstName is invalid and touched', () => {
      const control = component.personForm.get('firstName');
      control?.setValue('');
      control?.markAsTouched();
      fixture.detectChanges();

      const input = fixture.nativeElement.querySelector('#firstName');
      expect(input.classList).toContain('govuk-input--error');
    });
  });

  describe('Department Dropdown', () => {
    it('should display all departments in select options', () => {
      component.departments = mockDepartments;
      fixture.detectChanges();

      const options = fixture.nativeElement.querySelectorAll('#departmentId option');
      expect(options.length).toBe(mockDepartments.length + 1); // +1 for the default option

      mockDepartments.forEach((dept, index) => {
        expect(options[index + 1].value).toBe(dept.id.toString());
        expect(options[index + 1].textContent).toContain(dept.name);
      });
    });
  });
});
