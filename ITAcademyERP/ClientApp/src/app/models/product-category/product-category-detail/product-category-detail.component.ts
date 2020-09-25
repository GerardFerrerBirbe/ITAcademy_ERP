import { Component, OnInit } from '@angular/core';
import { ProductCategory } from '../product-category';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ProductCategoryService }  from '../../product-category/product-category.service';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-product-category-detail',
  templateUrl: './product-category-detail.component.html',
  styleUrls: ['./product-category-detail.component.css']
})
export class ProductCategoryDetailComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private productCategoryService: ProductCategoryService,
    private location: Location
  ) { }

  editionMode: boolean = false;
  formGroup: FormGroup;
  productCategoryId: any;

  productCategories: ProductCategory[];

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      productCategoryName: ''
    });  

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.productCategoryId = params["id"];

      this.productCategoryService.getProductCategory(this.productCategoryId.toString())
      .subscribe(productCategory => this.loadForm(productCategory));
    });
  }

  loadForm(productCategory: ProductCategory){
    this.formGroup.patchValue({
      productCategoryName: productCategory.productCategoryName
    })
  }

  save() {
    let productCategory: ProductCategory = Object.assign({}, this.formGroup.value);
    console.table(productCategory);

    if (this.editionMode){
      //edit productCategory     
      this.productCategoryId = parseInt(this.productCategoryId);
      productCategory.id = this.productCategoryId;      
      this.productCategoryService.updateProductCategory(productCategory)
      .subscribe();
    } else {
      //add productCategory
      this.productCategoryService.addProductCategory(productCategory)
      .subscribe();
    }    
  }
  
  goBack(): void {
    this.location.back();
  }

}
