import { NgModule } from '@angular/core';
import { ChartsModule } from 'ng2-charts';

import { StatisticsService } from './statistics.service';
import { StatisticsComponent } from './statistics.component';
import { LineChartComponent } from './line-chart/line-chart.component';
import { LineChartProductsComponent } from './line-chart-products/line-chart-products.component';


@NgModule({
  declarations: [
    StatisticsComponent,
    LineChartComponent,
    LineChartProductsComponent
  ],
  imports: [
    ChartsModule
  ],
  providers: [
    StatisticsService
  ]
})
export class StatisticsModule { }
