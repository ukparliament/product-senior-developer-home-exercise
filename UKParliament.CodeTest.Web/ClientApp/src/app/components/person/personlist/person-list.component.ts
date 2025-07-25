import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PersonViewModel } from '../../../models/person-view-model';

@Component({
  selector: 'app-person-list',
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.css']
})
export class PersonListComponent {
  @Input() people: PersonViewModel[] = [];
  @Output() selectPerson = new EventEmitter<PersonViewModel>();
  @Output() deletePerson = new EventEmitter<number>();
}
