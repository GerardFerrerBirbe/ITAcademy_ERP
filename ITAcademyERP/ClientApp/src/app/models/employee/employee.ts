import { Address } from '../address/address';

export interface Employee {
  id: string;
  personId: string;
  email: string;
  firstName: string;
  lastName: string;
  position: string;
  salary: number;  
  addresses: Address[];
}