import { Component, OnInit } from '@angular/core';
import { Product } from '../product';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ProductService }  from '../../product/product.service';
import { ProductCategoryService }  from '../../product-category/product-category.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ProductCategory } from '../../product-category/product-category';
import { Errors} from '../../errors/errors';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private productService: ProductService,
    private productCategoryService: ProductCategoryService,
    private location: Location
  ) { }

  editionMode: boolean = false;
  formGroup: FormGroup;
  productId: any;

  products: Product[];
  productCategories: ProductCategory[];

  errors: Errors;

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      productName: '',
      productCategoryName: ''
    });

    this.productCategoryService.getProductCategories()
    .subscribe(productCategories => this.productCategories = productCategories);

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.productId = params["id"];

      this.productService.getProduct(this.productId)
      .subscribe(product => this.loadForm(product));
    });
  }

  loadForm(product: Product){
    this.formGroup.patchValue({
      productName: product.productName,
      productCategoryName: product.productCategoryName
    })
  }

  save() {
    this.errors = {};
    let product: Product = Object.assign({}, this.formGroup.value);
    console.table(product);

    if (this.editionMode){
      //edit product     
      product.id = this.productId;      
      this.productService.updateProduct(product)
      .subscribe(
        () => alert("Actualització realitzada"),
        error => {
          if (error.error.errors == undefined) {
            this.errors = error.error;
          } else {
            this.errors = error.error.errors;
          }          
        });
    } else {
      //add product
      this.productService.addProduct(product)
      .subscribe(
        product => alert("Producte " + product.productName + " creat correctament"),
        error => {
          if (error.error.errors == undefined) {
            this.errors = error.error;
          } else {
            this.errors = error.error.errors;
          }          
        });
    }    
  }
  
  goBack(): void {
    this.location.back();
  }

}
