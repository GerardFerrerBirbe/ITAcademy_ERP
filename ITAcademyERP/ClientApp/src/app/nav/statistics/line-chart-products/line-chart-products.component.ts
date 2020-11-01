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
    {data: [], label: 'Total vendes'},
    {data: [], label: ''}
  ];

  statsByDate: StatsByDate[];
  statsByDateAndProduct: StatsByDate[];

  initialDate: string;
  finalDate: string;
  initialYear: number;
  finalYear: number;
  initialMonth: number;
  finalMonth: number;
  productName: string;
  
  
  constructor(
    private statisticsService: StatisticsService
  ) { }

  ngOnInit(): void {
    
  }
  
  applicateDateFilters(){
    this.initialDate = (document.getElementById("initialDate") as HTMLInputElement).value;
    this.finalDate = (document.getElementById("finalDate") as HTMLInputElement).value;
    this.productName = (document.getElementById("productName") as HTMLInputElement).value;
    
    this.statisticsService.getSalesByDate(this.initialDate, this.finalDate)
      .subscribe(stats => {
        this.statsByDate = stats;
        
        this.statisticsService.getSalesByDateAndProduct(this.initialDate, this.finalDate, this.productName)
          .subscribe(stats => {
            this.statsByDateAndProduct = stats;

            this.setChartData(this.statsByDate, this.statsByDateAndProduct);          
          });        
      });  
  }

  setChartData(statsByDate: StatsByDate[], statsByDateAndProduct: StatsByDate[]){
    
    this.lineChartLabels = [];
    this.lineChartData = [
      {data: [], label: 'Total vendes'},
      {data: [], label: 'Vendes ' + this.productName}
    ];
    
    this.initialYear = parseInt(this.initialDate.split("-")[0]);
    this.initialMonth = parseInt(this.initialDate.split("-")[1]);

    this.finalYear = parseInt(this.finalDate.split("-")[0]);
    this.finalMonth = parseInt(this.finalDate.split("-")[1]);

    if (this.initialYear == this.finalYear) {
      for (let month = this.initialMonth; month <= this.finalMonth; month++) {
        let label = this.initialYear + "-" + month;
        this.lineChartLabels.push(label);
        this.pushChartData(statsByDate, statsByDateAndProduct, label);        
      }
    }else{
      for (let month = this.initialMonth; month <= 12; month++) {
        let label = this.initialYear + "-" + month;
        this.lineChartLabels.push(label);
        this.pushChartData(statsByDate, statsByDateAndProduct, label);       
      }
      if (this.finalYear > this.initialYear + 1) {        
        for (let year = this.initialYear + 1; year < this.finalYear; year++) {     
          for (let month = 1; month <= 12; month++) {
            let label = year + "-" + month;
            this.lineChartLabels.push(label);
            this.pushChartData(statsByDate, statsByDateAndProduct, label);        
          }      
        }
      }
      for (let month = 1; month <= this.finalMonth; month++) {
        let label = this.finalYear + "-" + month;
        this.lineChartLabels.push(label);
        this.pushChartData(statsByDate, statsByDateAndProduct, label);         
      }      
    }
  }

  pushChartData (statsByDate: StatsByDate[], statsByDateAndProduct: StatsByDate[], label: string) {    
    let resultByDate = statsByDate.filter(s => s.yearMonth == label);
    if (resultByDate.length > 0) {
      this.lineChartData[0].data.push(resultByDate[0].totalSales);
    }else{
      this.lineChartData[0].data.push(0);
    }

    let resultByDateAndProduct = statsByDateAndProduct.filter(s => s.yearMonth == label);
    if (resultByDateAndProduct.length > 0) {
      this.lineChartData[1].data.push(resultByDateAndProduct[0].totalSales);
    }else{
      this.lineChartData[1].data.push(0);
    }
      
  }

  
}
