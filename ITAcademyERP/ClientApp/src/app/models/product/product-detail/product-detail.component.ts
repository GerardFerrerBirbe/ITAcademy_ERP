import { Component, OnInit } from '@angular/core';
import { Product } from '../product';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ProductService }  from '../../../services/product.service';
import { FormGroup, FormBuilder } from '@angular/forms';

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
    private location: Location
  ) { }

  editionMode: boolean = false;
  formGroup: FormGroup;
  productId: any;

  products: Product[];

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      productName: '',
      productCategoryId: ''
    });  

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.productId = params["id"];

      this.productService.getProduct(this.productId.toString())
      .subscribe(product => this.loadForm(product));
    });
  }

  loadForm(product: Product){
    this.formGroup.patchValue({
      productName: product.productName,
      productCategoryId: product.productCategoryId
    })
  }

  save() {
    let product: Product = Object.assign({}, this.formGroup.value);
    console.table(product);

    if (this.editionMode){
      //edit product     
      this.productId = parseInt(this.productId);
      product.id = this.productId;      
      this.productService.updateProduct(product)
      .subscribe();
    } else {
      //add product
      this.productService.addProduct(product)
      .subscribe();
    }    
  }
  
  goBack(): void {
    this.location.back();
  }

}
