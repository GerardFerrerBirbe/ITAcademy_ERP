export interface OrderLine {
  id: number;
  orderHeaderId: number;
  productId: number;
  unitPrice: number;
  vat: string;
  quantity: string
}