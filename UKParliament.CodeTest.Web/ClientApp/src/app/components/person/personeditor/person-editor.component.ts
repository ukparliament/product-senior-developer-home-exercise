import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PersonViewModel } from '../../../models/person-view-model';
import { Department } from '../../../models/department-view-model';

@Component({
  selector: 'app-person-editor',
  templateUrl: './person-editor.component.html',
  styleUrls: ['./person-editor.component.css']
})
export class PersonEditorComponent {
  @Input() person!: PersonViewModel;
  @Input() departments: Department[] = [];
  @Output() savePerson = new EventEmitter<PersonViewModel>();
  personForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.personForm = this.fb.group({
      id: [0],
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      dateOfBirth: ['', Validators.required],
      departmentId: [0, Validators.min(1)],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]]
    });
  }

  ngOnChanges(): void {
    if (this.person) {
      this.personForm.patchValue({
        ...this.person,
        dateOfBirth: this.person.dateOfBirth || new Date().toISOString().split('T')[0]
      });
    }
  }

  save(): void {
    if (this.personForm.valid) {
      this.savePerson.emit(this.personForm.value);
    }
  }

  getErrorMessage(controlName: string): string {
    const control = this.personForm.get(controlName);
    if (control?.hasError('required')) return `${controlName} is required`;
    if (control?.hasError('maxlength')) return `${controlName} exceeds maximum length`;
    if (control?.hasError('email')) return 'Invalid email format';
    if (control?.hasError('min')) return 'Please select a department';
    return '';
  }
}
