import { Person } from '../person/person';

export interface Employee {
  id: string;
  personId: string;
  person: Person;
  position: string;
  salary: number;
}