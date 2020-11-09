import { Client } from '../client/client';
import { Employee } from '../employee/employee';
import { OrderLine } from '../order-line/order-line';
import { OrderState} from '../order-state/order-state';
import { OrderPriority} from '../order-priority/order-priority';

export interface OrderHeader {
  id: string;
  orderNumber: string;
  clientId: string;
  client: Client;
  employeeId: string;
  employee: Employee;
  orderState: OrderState;
  orderPriority: OrderPriority;
  creationDate: string;
  assignToEmployeeDate: string;
  finalisationDate: string;
  orderLines: OrderLine[];
}