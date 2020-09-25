import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { OrderHeader } from './order-header';

@Injectable({
  providedIn: 'root'
})
export class OrderHeaderService {

  private apiUrl = 'api/OrderHeaders';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }

  constructor(private http: HttpClient) { }

  getOrderHeaders(): Observable<OrderHeader[]>{
    return this.http.get<OrderHeader[]>(this.apiUrl);
  }

  getOHByEmployee(employeeId: number): Observable<OrderHeader[]>{
    let params = new HttpParams().set('employeeId', employeeId.toString());    
    return this.http.get<OrderHeader[]>(this.apiUrl + "/Employee", {params: params});
  }

  getOHByClient(clientId: number): Observable<OrderHeader[]>{
    let params = new HttpParams().set('clientId', clientId.toString());    
    return this.http.get<OrderHeader[]>(this.apiUrl + "/Client", {params: params});
  }
  
  getOrderHeader(id: string): Observable<OrderHeader> {
    let params = new HttpParams().set('includeOrderLines', "true");
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<OrderHeader>(url, {params: params});
  }

  updateOrderHeader(orderHeader: OrderHeader): Observable<OrderHeader> {
    return this.http.put<OrderHeader>(this.apiUrl + "/" + orderHeader.id.toString(), orderHeader, this.httpOptions);
  }

  addOrderHeader(orderHeader: OrderHeader): Observable<OrderHeader> {
    return this.http.post<OrderHeader>(this.apiUrl, orderHeader, this.httpOptions);
  }

  deleteOrderHeader(orderHeader: OrderHeader | number): Observable<OrderHeader> {
    const id = typeof orderHeader === 'number' ? orderHeader : orderHeader.id;
    const url = `${this.apiUrl}/${id}`;

    return this.http.delete<OrderHeader>(url, this.httpOptions);
  }
}
