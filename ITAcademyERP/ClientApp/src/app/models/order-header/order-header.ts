import { OrderLine } from '../order-line/order-line';
import { Address } from '../address/address';
import { Employee } from '../employee/employee';
import { Client } from '../client/client';
import { OrderState } from '../order-state/order-state';
import { OrderPriority } from '../order-priority/order-priority';

export interface OrderHeader {
  id: number;
  orderNumber: number;
  address: string;
  client: string;
  employee: string;
  orderState: string;
  orderPriority: string;
  creationDate: string;
  assignToEmployeeDate: string;
  finalisationDate: string;
  orderLines: OrderLine[];
}