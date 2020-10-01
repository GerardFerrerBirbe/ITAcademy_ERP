import { Address } from '../address/address'

export interface Client {
  id: number;
  personId: string;
  email: string;
  firstName: string;
  lastName: string;
  addresses: Address[];
  }