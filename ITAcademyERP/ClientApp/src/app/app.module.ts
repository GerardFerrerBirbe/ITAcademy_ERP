import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule} from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
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
import { OrderLineComponent } from './models/order-line/order-line.component';
import { OrderPriorityComponent } from './models/order-priority/order-priority.component';
import { OrderStateComponent } from './models/order-state/order-state.component';

import { AddressDetailComponent } from './models/address/address-detail/address-detail.component';
import { EmployeeDetailComponent } from './models/employee/employee-detail/employee-detail.component';
import { ClientDetailComponent } from './models/client/client-detail/client-detail.component';
import { PersonDetailComponent } from './models/person/person-detail/person-detail.component';
import { ProductDetailComponent } from './models/product/product-detail/product-detail.component';
import { ProductCategoryDetailComponent } from './models/product-category/product-category-detail/product-category-detail.component';
import { OrderDetailComponent } from './models/order-Header/order-detail/order-detail.component';
import { RoleComponent } from './models/role/role.component';
import { RoleDetailComponent } from './models/role/role-detail/role-detail.component';

import { EmployeeService } from './models/employee/employee.service';
import { ClientService } from './models/client/client.service';
import { PersonService } from './models/person/person.service';
import { ProductService } from './models/product/product.service';
import { ProductCategoryService } from './models/product-category/product-category.service';
import { AddressService } from './models/address/address.service';
import { OrderHeaderService } from './models/order-header/order-header.service';
import { OrderLineService } from './models/order-line/order-line.service';
import { OrderPriorityService } from './models/order-priority/order-priority.service';
import { OrderStateService } from './models/order-state/order-state.service';
import { LoginComponent } from './login/login.component';
import { AuthGuardService } from './services/auth-guard.service';
import { AccountService } from './login/account.service';
import { AuthInterceptorService } from './services/auth-interceptor.service';
import { LogInterceptorService } from './services/log-interceptor.service';
import { DatePipe } from '@angular/common';
import { RoleService } from './models/role/role.service';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    HhrrComponent,
    SalesComponent,
    StatisticsComponent,   
    EmployeeComponent,
    ClientComponent,
    AddressComponent,
    ProductComponent,
    ProductCategoryComponent,
    PersonComponent,
    OrderHeaderComponent,
    OrderLineComponent,
    OrderPriorityComponent,
    OrderStateComponent,
    AddressDetailComponent,
    EmployeeDetailComponent,
    ClientDetailComponent,
    PersonDetailComponent,
    ProductDetailComponent,
    ProductCategoryDetailComponent,
    OrderDetailComponent,
    LoginComponent,
    RoleComponent,
    RoleDetailComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    DatePipe,
    EmployeeService,
    PersonService,
    ClientService,
    AddressService,
    ProductService,
    ProductCategoryService,
    OrderHeaderService,
    OrderLineService,
    OrderPriorityService,
    OrderStateService,
    RoleService,
    AuthGuardService,
    AccountService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LogInterceptorService,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
