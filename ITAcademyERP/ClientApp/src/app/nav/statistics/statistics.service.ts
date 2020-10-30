import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Client } from 'src/app/models/client/client';
import { Product } from 'src/app/models/product/product';
import { StatsByDate } from './statsByDate';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  private apiUrl = 'api/OrderLines';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }
  
  constructor(private http: HttpClient) { }

  getTopProducts(): Observable<Product[]>{
    return this.http.get<Product[]>(this.apiUrl + "/TopProducts");
  }

  getTopClients(): Observable<Client[]>{
    return this.http.get<Client[]>(this.apiUrl + "/TopClients");
  }

  getSalesByDate(initialDate: string, finalDate: string): Observable<StatsByDate[]>{
    let params = new HttpParams()
      .set('initialDate', initialDate)
      .set('finalDate', finalDate);
    return this.http.get<StatsByDate[]>(this.apiUrl + "/SalesByDate", {params: params});
  }

  getSalesByDateAndProduct(): Observable<StatsByDate[]>{
    return this.http.get<StatsByDate[]>(this.apiUrl + "/SalesByDateAndProduct");
  }

}
