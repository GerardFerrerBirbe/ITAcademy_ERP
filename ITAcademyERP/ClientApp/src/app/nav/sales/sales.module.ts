import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from '../../app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { SalesComponent } from '../../nav/sales/sales.component';

import { ClientComponent } from '../../models/client/client.component';
import { ProductComponent } from '../../models/product/product.component';
import { ProductCategoryComponent } from '../../models/product-category/product-category.component';
import { OrderLineComponent } from '../../models/order-line/order-line.component';
import { OrderStateComponent } from '../../models/order-state/order-state.component';
import { OrderPriorityComponent } from '../../models/order-priority/order-priority.component';
import { ClientDetailComponent } from '../../models/client/client-detail/client-detail.component';
import { ProductDetailComponent } from '../../models/product/product-detail/product-detail.component';
import { ProductCategoryDetailComponent } from '../../models/product-category/product-category-detail/product-category-detail.component';

import { ClientService } from '../../models/client/client.service';
import { ProductService } from '../../models/product/product.service';
import { ProductCategoryService } from '../../models/product-category/product-category.service';
import { OrderLineService } from '../../models/order-line/order-line.service';
import { OrderPriorityService } from '../../models/order-priority/order-priority.service';
import { OrderStateService } from '../../models/order-state/order-state.service';


@NgModule({
  declarations: [
    SalesComponent,
    ClientComponent,
    ProductComponent,
    ProductCategoryComponent,
    OrderLineComponent,
    OrderPriorityComponent,
    OrderStateComponent,
    ClientDetailComponent,
    ProductDetailComponent,
    ProductCategoryDetailComponent,
    
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    ClientService,
    ProductService,
    ProductCategoryService,
    OrderLineService,
    OrderPriorityService,
    OrderStateService
  ]
})
export class SalesModule { }
