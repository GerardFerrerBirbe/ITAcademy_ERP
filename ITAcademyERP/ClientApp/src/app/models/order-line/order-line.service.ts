import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Client } from '../client/client';
import { OrderHeader } from '../order-header/order-header';
import { Product } from '../product/product';
import { OrderLine } from './order-line';

@Injectable({
  providedIn: 'root'
})
export class OrderLineService {

  private apiUrl = 'api/OrderLines';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }

  constructor(private http: HttpClient) { }

  getOrderLines(): Observable<OrderLine[]>{
    return this.http.get<OrderLine[]>(this.apiUrl);
  }
  
  addOrderLine(orderLine: OrderLine): Observable<OrderLine> {
    return this.http.post<OrderLine>(this.apiUrl, orderLine, this.httpOptions);
  }

  deleteOrderLine(orderLine: OrderLine | string): Observable<OrderLine> {
    const id = typeof orderLine === 'string' ? orderLine : orderLine.id;
    let url = `${this.apiUrl}/generic/${id}`;

    return this.http.delete<OrderLine>(url, this.httpOptions);
  }

  getTopProducts(): Observable<Product[]>{
    return this.http.get<Product[]>(this.apiUrl + "/TopProducts");
  }

  getTopClients(): Observable<Client[]>{
    return this.http.get<Client[]>(this.apiUrl + "/TopClients");
  }

  // getSalesByDate(): Observable<OrderHeader[]>{
  //   return this.http.get<OrderHeader[]>(this.apiUrl + "/SalesEvolution");
  // }

  getSalesByDateAndProduct(): Observable<Product[]>{
    return this.http.get<Product[]>(this.apiUrl + "/SalesByDateAndProduct");
  }
    
}
