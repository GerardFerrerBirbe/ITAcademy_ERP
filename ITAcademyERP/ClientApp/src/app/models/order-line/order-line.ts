export interface OrderLine {
  id: number;
  orderHeaderId: number;
  productName: string;
  unitPrice: number;
  vat: string;
  quantity: string
}