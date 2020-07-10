import { OrderLine } from '../order-line/order-line'

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
}