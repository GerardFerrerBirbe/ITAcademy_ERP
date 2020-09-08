import { OrderLine } from '../order-line/order-line';

export interface OrderHeader {
  id: number;
  orderNumber: string;
  addressId: number;
  address: string;
  clientId: number;
  client: string;
  employeeId: number;
  employee: string;
  orderStateId: number;
  orderState: string;
  orderPriorityId: number;
  orderPriority: string;
  creationDate: string;
  assignToEmployeeDate: string;
  finalisationDate: string;
  orderLines: OrderLine[];
}