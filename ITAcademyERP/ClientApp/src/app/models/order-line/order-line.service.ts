import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
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

  deleteOrderLine(orderLine: OrderLine | number): Observable<OrderLine> {
    const id = typeof orderLine === 'number' ? orderLine : orderLine.id;
    let url = `${this.apiUrl}/generic/${id}`;

    return this.http.delete<OrderLine>(url, this.httpOptions);
  }
    
}
