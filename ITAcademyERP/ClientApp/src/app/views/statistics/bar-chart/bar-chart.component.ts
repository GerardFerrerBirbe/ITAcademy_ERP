import { Component, OnInit } from '@angular/core';
import { StatisticsService } from '../statistics.service';

@Component({
  selector: 'app-bar-chart',
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.css']
})
export class BarChartComponent implements OnInit {

  public barChartOptions = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  public barChartLabels = [];
  public barChartType = 'bar';
  public barChartLegend = true;
  public barChartData = [
    {data: [], label: 'Vendes per client'}
  ];
  
  constructor(
    private statisticsService: StatisticsService
  ) { }

  ngOnInit(): void {
    this.statisticsService.getSalesByClient()
    .subscribe(stats => {
      for (let index = 0; index < stats.length; index++) {
        const element = stats[index];
        this.barChartLabels.push(element.firstName + " " + element.lastName);
        this.barChartData[0].data.push(element.totalSales);
      }
    })
  }

}
