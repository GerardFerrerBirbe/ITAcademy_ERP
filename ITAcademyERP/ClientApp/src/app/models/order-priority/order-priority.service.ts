import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { OrderPriority } from './order-priority';

@Injectable({
  providedIn: 'root'
})
export class OrderPriorityService {

  private apiUrl = 'api/orderpriorities';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }

  constructor(private http: HttpClient) { }

  getOrderPriorities(): Observable<OrderPriority[]>{
    return this.http.get<OrderPriority[]>(this.apiUrl);
  }
}
