import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OrderHeader } from '../models/order-header/order-header';

@Injectable({
  providedIn: 'root'
})
export class OhByClientService {

  private apiUrl = 'api/OHByClient';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }
  
  constructor(private http: HttpClient) { }

  getOHByClient(clientId: number): Observable<OrderHeader[]>{
    let params = new HttpParams().set('clientId', clientId.toString());    
    return this.http.get<OrderHeader[]>(this.apiUrl, {params: params});
  }
}
