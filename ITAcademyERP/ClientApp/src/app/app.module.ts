import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule} from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { DashboardComponent } from './nav/dashboard/dashboard.component';
import { StatisticsComponent } from './nav/statistics/statistics.component';
import { HhrrModule } from './nav/hhrr/hhrr.module';
import { SalesModule } from './nav/sales/sales.module';

import { AddressComponent } from './models/address/address.component';
import { OrderHeaderComponent } from './models/order-header/order-header.component';
import { OrderDetailComponent } from './models/order-Header/order-detail/order-detail.component';
import { LoginComponent } from './login/login.component';

import { AddressService } from './models/address/address.service';
import { AuthGuardService } from './services/auth-guard.service';
import { AccountService } from './login/account.service';
import { OrderHeaderService } from './models/order-header/order-header.service';

import { AuthInterceptorService } from './services/auth-interceptor.service';
import { LogInterceptorService } from './services/log-interceptor.service';
import { LineChartProductsComponent } from './nav/statistics/line-chart-products/line-chart-products.component';
import { BarChartComponent } from './nav/statistics/bar-chart/bar-chart.component';
import { DoughnutChartComponent } from './nav/statistics/doughnut-chart/doughnut-chart.component';
import { ChartsModule } from 'ng2-charts';
import { StatisticsService } from './nav/statistics/statistics.service';


@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    AddressComponent,
    LoginComponent,
    OrderHeaderComponent,
    OrderDetailComponent,
    StatisticsComponent,
    LineChartProductsComponent,
    BarChartComponent,
    DoughnutChartComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    HhrrModule,
    SalesModule,
    ChartsModule
  ],
  providers: [
    AddressService,
    StatisticsService,
    OrderHeaderService,
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
