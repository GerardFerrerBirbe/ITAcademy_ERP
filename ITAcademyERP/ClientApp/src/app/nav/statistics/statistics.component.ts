import { Component, OnInit } from '@angular/core';
import { Client } from 'src/app/models/client/client';
import { ClientService } from 'src/app/models/client/client.service';
import { OrderHeader } from 'src/app/models/order-header/order-header';
import { OrderLine } from 'src/app/models/order-line/order-line';
import { OrderLineService } from 'src/app/models/order-line/order-line.service';
import { Product } from 'src/app/models/product/product';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements OnInit {

  totalSales: number;
  totalClients: number;

  products: Product[];
  clients: Client[];
  orderHeaders: OrderHeader[];
  
  public lineChartOptions = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  public lineChartLabels = [];
  public lineChartType = 'line';
  public lineChartLegend = true;
  public lineChartData = [
    {data: [], label: 'Evolució vendes'}
  ];
  
  constructor(
    private clientService: ClientService,
    private orderLineService: OrderLineService
  ) { }

  ngOnInit(): void {
    this.clientService.getClients()
      .subscribe(clients =>
        this.calculateTotalClients(clients));
    
    this.orderLineService.getOrderLines()
      .subscribe(orderLines =>
        this.calculateTotalSales(orderLines));

    this.orderLineService.getTopProducts()
      .subscribe(products => this.products = products)
    
    this.orderLineService.getTopClients()
      .subscribe(clients => this.clients = clients)

    this.orderLineService.getSalesByDate()
      .subscribe(orderHeaders => {
        this.setLineChartData(orderHeaders);
      })
    }

  calculateTotalClients(clients: Client[]): void {
    this.totalClients = clients.length;        
  }  
  
  calculateTotalSales(orderLines: OrderLine[]): void {
    this.totalSales = 0;

    orderLines.forEach(orderLine => {
      this.totalSales += orderLine.unitPrice * orderLine.quantity * (1 + orderLine.vat);
    });   
  }

  setLineChartData(orderHeaders : OrderHeader[]){
    orderHeaders.forEach(orderHeader => {
      this.lineChartLabels.push(orderHeader.yearMonth);
      this.lineChartData[0].data.push(orderHeader.totalSales);
    });
  }
}
