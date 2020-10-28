import { OrderLine } from '../order-line/order-line';

export interface OrderHeader {
  id: string;
  orderNumber: string;
  address: string;
  client: string;
  employee: string;
  orderState: string;
  orderPriority: string;
  creationDate: string;
  assignToEmployeeDate: string;
  finalisationDate: string;
  yearMonth: string;
  totalSales: number;
  orderLines: OrderLine[];
}