import { Component, OnInit } from '@angular/core';
import { Product } from '../product';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ProductService }  from '../../product/product.service';
import { ProductCategoryService }  from '../../product-category/product-category.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ProductCategory } from '../../product-category/product-category';
import { Guid } from 'guid-typescript';

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

  errors: any;

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      name: '',
      categoryName: ''
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
      name: product.name,
      categoryName: product.category.name
    })
  }

  save() {
    this.errors = {};
    //let product: Product = Object.assign({}, this.formGroup.value;
    let product: Product = {
      id: Guid.EMPTY,
      name: this.formGroup.get('name').value,
      category: <ProductCategory>{
        id: Guid.EMPTY,
        name: this.formGroup.get('categoryName').value,
      }
    };
    console.table(product);

    if (this.editionMode){
      //edit product     
      product.id = this.productId; 
      this.productService.updateProduct(product)
      .subscribe(
        () => alert("Actualització realitzada"),
        error => {
            if (error.error == null) {
              alert(error.status + " Usuari no autoritzat");                            
            } else if (error.error.errors == undefined) {
              this.errors = error.error;
            } else {
              this.errors = error.error.errors;
            }
          });
    } else {
      //add product
      this.productService.addProduct(product)
      .subscribe(
        product => alert("Producte " + product.name + " creat correctament"),
        error => {
            if (error.error == null) {
              alert(error.status + " Usuari no autoritzat");                            
            } else if (error.error.errors == undefined) {
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
