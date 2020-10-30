import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { StatisticsService } from '../statistics.service';
import { StatsByDate } from '../statsByDate';

@Component({
  selector: 'app-line-chart-products',
  templateUrl: './line-chart-products.component.html',
  styleUrls: ['./line-chart-products.component.css']
})
export class LineChartProductsComponent implements OnInit {

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

  initialDate: string;
  finalDate: string;
  initialYear: number;
  finalYear: number;
  initialMonth: number;
  finalMonth: number;
  
  
  constructor(
    private statisticsService: StatisticsService
  ) { }

  ngOnInit(): void {
    
  }

  
  applicateDateFilters(){
    this.initialDate = (document.getElementById("initialDate") as HTMLInputElement).value;
    this.finalDate = (document.getElementById("finalDate") as HTMLInputElement).value;
    
    this.statisticsService.getSalesByDate(this.initialDate, this.finalDate)
      .subscribe(statsByDate => {
        this.setLabelFilters(statsByDate);
      });        
  }

  setLabelFilters(statsByDate: StatsByDate[]){
    
    this.lineChartLabels = [];
    this.lineChartData = [
      {data: [], label: 'Total vendes'}
    ];
    
    this.initialYear = parseInt(this.initialDate.split("-")[0]);
    this.initialMonth = parseInt(this.initialDate.split("-")[1]);

    this.finalYear = parseInt(this.finalDate.split("-")[0]);
    this.finalMonth = parseInt(this.finalDate.split("-")[1]);

    if (this.initialYear == this.finalYear) {
      for (let month = this.initialMonth; month <= this.finalMonth; month++) {
        let label = this.initialYear + "-" + month;
        this.lineChartLabels.push(label);
        this.pushChartData(statsByDate, label);        
      }
    }else{
      for (let month = this.initialMonth; month <= 12; month++) {
        let label = this.initialYear + "-" + month;
        this.lineChartLabels.push(label);
        this.pushChartData(statsByDate, label);       
      }
      if (this.finalYear > this.initialYear + 1) {        
        for (let year = this.initialYear + 1; year < this.finalYear; year++) {     
          for (let month = 1; month <= 12; month++) {
            let label = year + "-" + month;
            this.lineChartLabels.push(label);
            this.pushChartData(statsByDate, label);        
          }      
        }
      }
      for (let month = 1; month <= this.finalMonth; month++) {
        let label = this.finalYear + "-" + month;
        this.lineChartLabels.push(label);
        this.pushChartData(statsByDate, label);         
      }      
    }
  }

  pushChartData (statsByDate: StatsByDate[], label: string) {    
    let result = statsByDate.filter(s => s.yearMonth == label);
    if (result.length > 0) {
      this.lineChartData[0].data.push(result[0].totalSales);
    }else{
      this.lineChartData[0].data.push(0);
    }    
  }

  
}
