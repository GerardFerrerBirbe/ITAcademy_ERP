import { ProductCategory } from '../product-category/product-category';

export interface Product {
  id: string;
  name: string;
  category: ProductCategory;
}