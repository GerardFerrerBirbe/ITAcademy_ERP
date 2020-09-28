import { Address } from '../address/address';

export interface Employee {
  id: number;
  personId: number;
  email: string;
  firstName: string;
  lastName: string;
  position: string;
  salary: number;  
  addresses: Address[];
}