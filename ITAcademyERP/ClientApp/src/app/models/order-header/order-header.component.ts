import { Component, OnInit } from '@angular/core';
import { OrderHeader } from './order-header';
import { OrderHeaderService } from './order-header.service';


@Component({
  selector: 'app-order-header',
  templateUrl: './order-header.component.html',
  styleUrls: ['./order-header.component.css']
})
export class OrderHeaderComponent implements OnInit {

  public orderHeaders: OrderHeader[];
  
  constructor(
    private orderHeaderService: OrderHeaderService,
    ) { }

  ngOnInit(): void {
    this.getOrderHeaders();
  } 

  getOrderHeaders(): void {
    this.orderHeaderService.getOrderHeaders()
    .subscribe(orderHeaders => this.orderHeaders = orderHeaders);
  }

  delete(orderHeader: OrderHeader): void {
    this.orderHeaders = this.orderHeaders.filter(e => e !== orderHeader);
    this.orderHeaderService.deleteOrderHeader(orderHeader).subscribe();
  }
}
