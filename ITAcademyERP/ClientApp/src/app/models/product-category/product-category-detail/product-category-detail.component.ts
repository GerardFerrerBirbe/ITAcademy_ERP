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

  errors: any;

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      name: ''
    });  

    this.route.params.subscribe(params => {
      if (params["id"] == undefined){
        return;
      }
      this.editionMode = true;

      this.productCategoryId = params["id"];

      this.productCategoryService.getProductCategory(this.productCategoryId)
      .subscribe(productCategory => this.loadForm(productCategory));
    });
  }

  loadForm(productCategory: ProductCategory){
    this.formGroup.patchValue({
      name: productCategory.name
    })
  }

  save() {
    this.errors = {};
    let productCategory: ProductCategory = Object.assign({}, this.formGroup.value);
    console.table(productCategory);

    if (this.editionMode){
      //edit productCategory     
      productCategory.id = this.productCategoryId;      
      this.productCategoryService.updateProductCategory(productCategory)
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
      //add productCategory
      this.productCategoryService.addProductCategory(productCategory)
      .subscribe(
        pc => alert("Categoria " + pc.name + " creada correctament"),
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
