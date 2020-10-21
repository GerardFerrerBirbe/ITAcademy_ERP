export interface OrderLine {
  id: string;
  orderHeaderId: string;
  productName: string;
  unitPrice: number;
  vat: number;
  quantity: number
}