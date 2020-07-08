import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './nav/dashboard/dashboard.component';
import { HhrrComponent } from './nav/hhrr/hhrr.component';
import { SalesComponent } from './nav/sales/sales.component';
import { StatisticsComponent } from './nav/statistics/statistics.component';

import { EmployeeComponent } from './models/employee/employee.component';
import { ClientComponent } from './models/client/client.component';
import { AddressComponent } from './models/address/address.component';
import { ProductComponent } from './models/product/product.component';
import { ProductCategoryComponent } from './models/product-category/product-category.component';
import { PersonComponent } from './models/person/person.component';
import { OrderHeaderComponent } from './models/order-header/order-header.component';

import { AddressDetailComponent } from './models/address/address-detail/address-detail.component';
import { EmployeeDetailComponent } from './models/employee/employee-detail/employee-detail.component';
import { ClientDetailComponent } from './models/client/client-detail/client-detail.component';
import { PersonDetailComponent } from './models/person/person-detail/person-detail.component';
import { ProductDetailComponent } from './models/product/product-detail/product-detail.component';
import { ProductCategoryDetailComponent } from './models/product-category/product-category-detail/product-category-detail.component';
import { OrderDetailComponent } from './models/order-Header/order-detail/order-detail.component';

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'hhrr', component: HhrrComponent },
  { path: 'sales', component: SalesComponent },
  { path: 'statistics', component: StatisticsComponent },
  { path: 'employee', component: EmployeeComponent },
  { path: 'employee-form', component: EmployeeDetailComponent },
  { path: 'employee-form/:id', component: EmployeeDetailComponent },
  { path: 'client', component: ClientComponent },
  { path: 'client-form', component: ClientDetailComponent },
  { path: 'client-form/:id', component: ClientDetailComponent },
  { path: 'person', component: PersonComponent },
  { path: 'person-form', component: PersonDetailComponent },
  { path: 'person-form/:id', component: PersonDetailComponent },
  { path: 'address', component: AddressComponent },
  { path: 'address-form', component: AddressDetailComponent },
  { path: 'address-form/:id', component: AddressDetailComponent },
  { path: 'order', component: OrderHeaderComponent },
  { path: 'order-form', component: OrderDetailComponent },
  { path: 'order-form/:id', component: OrderDetailComponent },
  { path: 'product', component: ProductComponent },
  { path: 'product-form', component: ProductDetailComponent },
  { path: 'product-form/:id', component: ProductDetailComponent },
  { path: 'product-cat', component: ProductCategoryComponent },
  { path: 'product-cat-form', component: ProductCategoryDetailComponent },
  { path: 'product-cat-form/:id', component: ProductCategoryDetailComponent }  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
