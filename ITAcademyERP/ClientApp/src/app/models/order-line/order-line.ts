export interface OrderLine {
  id: number;
  orderHeaderId: number;
  productName: string;
  unitPrice: number;
  vat: number;
  quantity: number
}