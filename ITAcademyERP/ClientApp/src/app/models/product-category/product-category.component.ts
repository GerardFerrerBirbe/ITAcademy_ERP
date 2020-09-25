import { Component, OnInit } from '@angular/core';
import { ProductCategory } from './product-category';
import { ProductCategoryService } from 'src/app/models/product-category/product-category.service';


@Component({
  selector: 'app-product-category',
  templateUrl: './product-category.component.html',
  styleUrls: ['./product-category.component.css']
})
export class ProductCategoryComponent implements OnInit {

  public productCategories: ProductCategory[];
  
  constructor(private productCategoryService: ProductCategoryService) { }

  ngOnInit(): void {
    this.getProductCategories();
  }

  getProductCategories(): void {
    this.productCategoryService.getProductCategories()
    .subscribe(productCategories => this.productCategories = productCategories);
  }

  delete(productCategory: ProductCategory): void {
    this.productCategories = this.productCategories.filter(e => e !== productCategory);
    this.productCategoryService.deleteProductCategory(productCategory).subscribe();
  }
}