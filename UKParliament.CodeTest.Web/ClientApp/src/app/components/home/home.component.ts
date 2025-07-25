import { Component, OnInit } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { DepartmentService } from '../../services/department.service';
import { Department } from '../../models/department-view-model';
import { PersonViewModel } from '../../models/person-view-model';
import { ValidationError } from '../../models/validation-error';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  people: PersonViewModel[] = [];
  departments: Department[] = [];
  selectedPerson: PersonViewModel | null = null;
  errors: string[] = [];

  constructor(
    private personService: PersonService,
    private departmentService: DepartmentService
  ) { }

  ngOnInit(): void {
    this.loadPeople();
    this.loadDepartments();
  }

  loadPeople(): void {
    this.personService.getAll().subscribe({
      next: (people) => this.people = people,
      error: (err: ValidationError) => this.errors = err.errors
    });
  }

  loadDepartments(): void {
    this.departmentService.getAll().subscribe({
      next: (departments) => this.departments = departments,
      error: (err: ValidationError) => this.errors = err.errors
    });
  }

  selectPerson(person: PersonViewModel): void {
    this.selectedPerson = { ...person }; // Clone to avoid direct mutation
    this.errors = [];
  }

  addNewPerson(): void {
    this.selectedPerson = {
      id: 0,
      firstName: '',
      lastName: '',
      dateOfBirth: '',
      departmentId: 0,
      departmentName: '',
      email: ''
    };
    this.errors = [];
  }

  savePerson(person: PersonViewModel): void {
    const saveObservable = person.id === 0
      ? this.personService.add(person)
      : this.personService.update(person.id, person);

    saveObservable.subscribe({
      next: () => {
        this.loadPeople();
        this.selectedPerson = null;
        this.errors = [];
      },
      error: (err: string[]) => this.errors = err
    });
  }

  deletePerson(id: number): void {
    if (confirm('Are you sure you want to delete this person?')) {
      this.personService.delete(id).subscribe({
        next: () => {
          this.loadPeople();
          this.selectedPerson = null;
          this.errors = [];
        },
        error: (err: string[]) => this.errors = err
      });
    }
  }
}
