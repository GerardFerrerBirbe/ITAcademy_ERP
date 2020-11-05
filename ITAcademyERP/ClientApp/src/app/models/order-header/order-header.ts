import { Client } from '../client/client';
import { Employee } from '../employee/employee';
import { OrderLine } from '../order-line/order-line';
import { OrderState} from './orderState';
import { OrderPriority} from './orderPriority';

export interface OrderHeader {
  id: string;
  orderNumber: string;
  client: Client;
  employee: Employee;
  orderState: OrderState;
  orderPriority: OrderPriority;
  creationDate: string;
  assignToEmployeeDate: string;
  finalisationDate: string;
  orderLines: OrderLine[];
}