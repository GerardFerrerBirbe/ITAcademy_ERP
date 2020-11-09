import { Person } from '../person/person';

export interface Client {
  id: string;
  personId: string;
  person: Person;
  }