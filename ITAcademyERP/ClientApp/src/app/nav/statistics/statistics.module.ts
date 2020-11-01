import { NgModule } from '@angular/core';
import { ChartsModule } from 'ng2-charts';

import { StatisticsService } from './statistics.service';
import { StatisticsComponent } from './statistics.component';
import { LineChartComponent } from './line-chart/line-chart.component';
import { LineChartProductsComponent } from './line-chart-products/line-chart-products.component';
import { BarChartComponent } from './bar-chart/bar-chart.component';
import { DoughnutChartComponent } from './doughnut-chart/doughnut-chart.component';


@NgModule({
  declarations: [
    StatisticsComponent,
    LineChartComponent,
    LineChartProductsComponent,
    BarChartComponent,
    DoughnutChartComponent
  ],
  imports: [
    ChartsModule
  ],
  providers: [
    StatisticsService
  ]
})
export class StatisticsModule { }
