import { Component, OnInit } from '@angular/core';
import { Product } from './product';
import { ProductService } from 'src/app/models/product/product.service';


@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  public products: Product[];
  
  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts(): void {
    this.productService.getProducts()
    .subscribe(products => this.products = products);
  }

  delete(product: Product): void {
    this.products = this.products.filter(e => e !== product);
    this.productService.deleteProduct(product).subscribe();
  }
}
