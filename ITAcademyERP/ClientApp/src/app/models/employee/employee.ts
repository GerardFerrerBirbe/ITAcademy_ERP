import { Person } from '../person/person';

export interface Employee {
  id: number;
  personId: number;
  position: string;
  salary: number;
  userName: string;
  password: string
}