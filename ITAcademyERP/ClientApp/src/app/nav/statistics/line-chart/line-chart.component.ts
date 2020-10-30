import { Component, OnInit } from '@angular/core';
import { StatisticsService } from '../statistics.service';
import { StatsByDate } from '../statsByDate';

@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.css']
})
export class LineChartComponent implements OnInit {
  
  public lineChartOptions = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  public lineChartLabels = [];
  public lineChartType = 'line';
  public lineChartLegend = true;
  public lineChartData = [
    {data: [], label: 'Total vendes'}
  ];
  
  constructor(
    private statisticsService: StatisticsService
  ) { }

  ngOnInit(): void {

    // this.statisticsService.getSalesByDate()
    //   .subscribe(statsByDate => {
    //     this.setLineChartData(statsByDate);
    //   })

  }
  
  setLineChartData(statsByDate : StatsByDate[]){
    statsByDate.forEach(statByDate => {
      this.lineChartLabels.push(statByDate.yearMonth);
      this.lineChartData[0].data.push(statByDate.totalSales);
    });
  }

}
