import { Person } from '../person/person';

export interface Employee {
  id: string;
  person: Person;
  position: string;
  salary: number;
}