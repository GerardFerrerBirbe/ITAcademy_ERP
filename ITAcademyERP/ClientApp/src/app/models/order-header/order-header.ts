import { OrderLine } from '../order-line/order-line';
import { Address } from '../address/address';
import { Employee } from '../employee/employee';
import { Client } from '../client/client';
import { OrderState } from '../order-state/order-state';
import { OrderPriority } from '../order-priority/order-priority';

export interface OrderHeader {
  id: number;
  orderNumber: number;
  deliveryAddressId: number;
  clientId: number;
  employeeId: number;
  orderStateId: number;
  orderPriorityId: number;
  creationDate: string;
  assignToEmployeeDate: string;
  finalisationDate: string;
  orderLines: OrderLine[];
  deliveryAddress: Address;
  client: Client;
  employee: Employee;
  orderState: OrderState;
  orderPriority: OrderPriority

}