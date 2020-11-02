import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule} from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { DashboardComponent } from './views/dashboard/dashboard.component';
import { StatisticsComponent } from './views/statistics/statistics.component';
import { HhrrModule } from './views/hhrr/hhrr.module';
import { SalesModule } from './views/sales/sales.module';

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
import { LineChartProductsComponent } from './views/statistics/line-chart-products/line-chart-products.component';
import { BarChartComponent } from './views/statistics/bar-chart/bar-chart.component';
import { DoughnutChartComponent } from './views/statistics/doughnut-chart/doughnut-chart.component';
import { ChartsModule } from 'ng2-charts';
import { StatisticsService } from './views/statistics/statistics.service';
import { NavBarComponent } from './nav-bar/nav-bar.component';


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
    DoughnutChartComponent,
    NavBarComponent
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
    NavBarComponent,
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
