import { Person } from '../person/person';

export interface Employee {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  address: string;
  position: string;
  salary: number;
}