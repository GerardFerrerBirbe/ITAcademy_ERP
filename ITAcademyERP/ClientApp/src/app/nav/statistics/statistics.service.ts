import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StatsByDate } from './statsByDate';
import { StatsByClient } from './statsByClient';
import { StatsByProduct } from './statsByProduct';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  private apiUrl = 'api/OrderLines';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }
  
  constructor(private http: HttpClient) { }

  getTopProducts(): Observable<StatsByProduct[]>{
    return this.http.get<StatsByProduct[]>(this.apiUrl + "/TopProducts");
  }

  getSalesByProduct(): Observable<StatsByProduct[]>{
    return this.http.get<StatsByProduct[]>(this.apiUrl + "/SalesByProduct");
  }

  getTopClients(): Observable<StatsByClient[]>{
    return this.http.get<StatsByClient[]>(this.apiUrl + "/TopClients");
  }

  getSalesByClient(): Observable<StatsByClient[]>{
    return this.http.get<StatsByClient[]>(this.apiUrl + "/SalesByClient");
  }

  getSalesByDate(initialDate: string, finalDate: string): Observable<StatsByDate[]>{
    let params = new HttpParams()
      .set('initialDate', initialDate)
      .set('finalDate', finalDate);
    return this.http.get<StatsByDate[]>(this.apiUrl + "/SalesByDate", {params: params});
  }

  getSalesByDateAndProduct(initialDate: string, finalDate: string, productName: string): Observable<StatsByDate[]>{
    let params = new HttpParams()
      .set('initialDate', initialDate)
      .set('finalDate', finalDate)
      .set('productName', productName);
    return this.http.get<StatsByDate[]>(this.apiUrl + "/SalesByDateAndProduct", {params: params});
  }

}
