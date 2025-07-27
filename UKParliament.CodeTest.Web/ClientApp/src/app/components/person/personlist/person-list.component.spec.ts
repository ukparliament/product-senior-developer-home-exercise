import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { PersonListComponent } from './person-list.component';
import { PersonViewModel } from '../../../models/person-view-model';

describe('PersonListComponent', () => {
  let component: PersonListComponent;
  let fixture: ComponentFixture<PersonListComponent>;

  const mockPeople: PersonViewModel[] =[
      {
        id: 1,
        firstName: 'John',
        lastName: 'Doe',
        dateOfBirth: '1990-01-01',
        departmentId: 1,
        departmentName: 'IT',
        email: 'john@example.com'
      },
      {
        id: 2,
        firstName: 'Jane',
        lastName: 'Smith',
        dateOfBirth: '1992-05-15',
        departmentId: 2,
        departmentName: 'HR',
        email: 'jane@example.com'
      }
    ];


  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PersonListComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonListComponent);
    component = fixture.componentInstance;
    component.people = mockPeople;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display a table with correct headers', () => {
    const headers = fixture.debugElement.queryAll(By.css('th'));
    expect(headers.length).toBe(4);
    expect(headers[0].nativeElement.textContent).toContain('First Name');
    expect(headers[1].nativeElement.textContent).toContain('Last Name');
    expect(headers[2].nativeElement.textContent).toContain('Department');
    expect(headers[3].nativeElement.textContent).toContain('Actions');
  });

  it('should display all people in the table', () => {
    const rows = fixture.debugElement.queryAll(By.css('tbody tr'));
    expect(rows.length).toBe(mockPeople.length);

    // Check first row
    const firstRowCells = rows[0].queryAll(By.css('td'));
    expect(firstRowCells[0].nativeElement.textContent).toContain('John');
    expect(firstRowCells[1].nativeElement.textContent).toContain('Doe');
    expect(firstRowCells[2].nativeElement.textContent).toContain('IT');

    // Check second row
    const secondRowCells = rows[1].queryAll(By.css('td'));
    expect(secondRowCells[0].nativeElement.textContent).toContain('Jane');
    expect(secondRowCells[1].nativeElement.textContent).toContain('Smith');
    expect(secondRowCells[2].nativeElement.textContent).toContain('HR');
  });

  it('should display Edit and Delete buttons for each person', () => {
    const rows = fixture.debugElement.queryAll(By.css('tbody tr'));
    rows.forEach((row, index) => {
      const buttons = row.queryAll(By.css('button'));
      expect(buttons.length).toBe(2);

      // Edit button
      expect(buttons[0].nativeElement.textContent).toContain('Edit');
      expect(buttons[0].nativeElement.classList).toContain('govuk-button--secondary');
      expect(buttons[0].attributes['aria-label']).toContain(`Edit ${mockPeople[index].firstName} ${mockPeople[index].lastName}`);

      // Delete button
      expect(buttons[1].nativeElement.textContent).toContain('Delete');
      expect(buttons[1].nativeElement.classList).toContain('govuk-button--warning');
      expect(buttons[1].attributes['aria-label']).toContain(`Delete ${mockPeople[index].firstName} ${mockPeople[index].lastName}`);
    });
  });

  it('should emit selectPerson event when Edit button is clicked', () => {
    spyOn(component.selectPerson, 'emit');
    const editButton = fixture.debugElement.queryAll(By.css('button.govuk-button--secondary'))[0];
    
    editButton.triggerEventHandler('click', null);
    fixture.detectChanges();

    expect(component.selectPerson.emit).toHaveBeenCalledWith(mockPeople[0]);
  });

  it('should emit deletePerson event with correct id when Delete button is clicked', () => {
    spyOn(component.deletePerson, 'emit');
    const deleteButton = fixture.debugElement.queryAll(By.css('button.govuk-button--warning'))[0];
    
    deleteButton.triggerEventHandler('click', { stopPropagation: () => {} });
    fixture.detectChanges();

    expect(component.deletePerson.emit).toHaveBeenCalledWith(mockPeople[0].id);
  });

  it('should call stopPropagation when Delete button is clicked', () => {
    const stopPropagationSpy = jasmine.createSpy('stopPropagation');
    const deleteButton = fixture.debugElement.queryAll(By.css('button.govuk-button--warning'))[0];
    
    deleteButton.triggerEventHandler('click', { stopPropagation: stopPropagationSpy });
    fixture.detectChanges();

    expect(stopPropagationSpy).toHaveBeenCalled();
  });

  it('should display nothing when people array is empty', () => {
    component.people = [];
    fixture.detectChanges();

    const rows = fixture.debugElement.queryAll(By.css('tbody tr'));
    expect(rows.length).toBe(0);
  });
});
