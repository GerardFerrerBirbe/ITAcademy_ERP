import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OrderHeader } from '../models/order-header/order-header';

@Injectable({
  providedIn: 'root'
})
export class OhByEmployeeService {

  private apiUrl = 'api/OHByEmployee';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }
  
  constructor(private http: HttpClient) { }

  getOHByEmployee(employeeId: number): Observable<OrderHeader[]>{
    let params = new HttpParams().set('employeeId', employeeId.toString());    
    return this.http.get<OrderHeader[]>(this.apiUrl, {params: params});
  }
}
