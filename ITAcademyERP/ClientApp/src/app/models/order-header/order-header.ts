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
}