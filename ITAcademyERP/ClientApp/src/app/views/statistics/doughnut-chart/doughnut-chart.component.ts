import { Component, OnInit } from '@angular/core';
import { StatisticsService } from '../statistics.service';

@Component({
  selector: 'app-doughnut-chart',
  templateUrl: './doughnut-chart.component.html',
  styleUrls: ['./doughnut-chart.component.css']
})
export class DoughnutChartComponent implements OnInit {

  public doughnutChartLabels = [];
  public doughnutChartData = [];
  public doughnutChartType = 'doughnut';
  
  constructor(
    private statisticsService: StatisticsService
  ) { }

  ngOnInit(): void {
    this.statisticsService.getSalesByProduct()
    .subscribe(stats => {
      for (let index = 0; index < stats.length; index++) {
        const element = stats[index];
        this.doughnutChartLabels.push(element.productName);
        this.doughnutChartData.push(element.totalSales);
      }
    })
  }

}
