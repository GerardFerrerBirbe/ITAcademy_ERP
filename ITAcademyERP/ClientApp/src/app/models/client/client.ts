import { Address } from '../address/address'

export interface Client {
  id: string;
  personId: string;
  email: string;
  firstName: string;
  lastName: string;
  addresses: Address[];
  totalSales: number;
  }