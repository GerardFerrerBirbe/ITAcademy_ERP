import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { OrderState } from './order-state';

@Injectable({
  providedIn: 'root'
})
export class OrderStateService {

  private apiUrl = 'api/orderStates';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }

  constructor(private http: HttpClient) { }

  getOrderStates(): Observable<OrderState[]>{
    return this.http.get<OrderState[]>(this.apiUrl + "/generic/");
  }
}
