import { Component, OnInit } from '@angular/core';
import { Person } from './person';
import { PersonService } from './person.service';

@Component({
  selector: 'app-person',
  templateUrl: './person.component.html',
  styleUrls: ['./person.component.css']
})
export class PersonComponent implements OnInit {

  public people: Person[] = [];
  
  constructor(private personService: PersonService) { }

  ngOnInit(): void {
    var output = this.getPeople();
  }

  getPeople(): void {
    this.personService.getPeople()
    .subscribe(people => this.people = people);
  }

  delete(person: Person): void {
    this.people = this.people.filter(p => p !== person);
    this.personService.deletePerson(person).subscribe();
  }

}
