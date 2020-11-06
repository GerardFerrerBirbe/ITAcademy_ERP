import { Product } from '../product/product';

export interface OrderLine {
  id: string;
  orderHeaderId: string;
  product: Product;
  unitPrice: number;
  vat: number;
  quantity: number
}