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
import { OrderHeaderComponent } from './models/order-header/order-header.component';
import { RoleComponent } from './models/role/role.component';

import { EmployeeDetailComponent } from './models/employee/employee-detail/employee-detail.component';
import { ClientDetailComponent } from './models/client/client-detail/client-detail.component';
import { ProductDetailComponent } from './models/product/product-detail/product-detail.component';
import { ProductCategoryDetailComponent } from './models/product-category/product-category-detail/product-category-detail.component';
import { OrderDetailComponent } from './models/order-Header/order-detail/order-detail.component';

import { LoginComponent } from './login/login.component';
import { AuthGuardService } from './services/auth-guard.service';
import { RoleDetailComponent } from './models/role/role-detail/role-detail.component';

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'hhrr', component: HhrrComponent },
  { path: 'sales', component: SalesComponent },
  { path: 'statistics', component: StatisticsComponent },
  { path: 'employee', component: EmployeeComponent/*, canActivate: [AuthGuardService]*/ },
  { path: 'employee-detail', component: EmployeeDetailComponent },
  { path: 'employee-detail/:id', component: EmployeeDetailComponent },
  { path: 'client', component: ClientComponent },
  { path: 'client-detail', component: ClientDetailComponent },
  { path: 'client-detail/:id', component: ClientDetailComponent },
  { path: 'address', component: AddressComponent },
  { path: 'order', component: OrderHeaderComponent },
  { path: 'order-detail', component: OrderDetailComponent },
  { path: 'order-detail/:id', component: OrderDetailComponent },
  { path: 'product', component: ProductComponent },
  { path: 'product-detail', component: ProductDetailComponent },
  { path: 'product-detail/:id', component: ProductDetailComponent },
  { path: 'product-cat', component: ProductCategoryComponent },
  { path: 'product-cat-detail', component: ProductCategoryDetailComponent },
  { path: 'product-cat-detail/:id', component: ProductCategoryDetailComponent },
  { path: 'login', component: LoginComponent },
  { path: 'role', component: RoleComponent },
  { path: 'role-detail', component: RoleDetailComponent },
  { path: 'role-detail/:id', component: RoleDetailComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
