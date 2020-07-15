import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderLineService {

  private apiUrl = 'api/OrderLines';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }

  constructor(private http: HttpClient) { }

  deleteOrderLines(ids: number[]): Observable<void>{
    return this.http.post<void>(this.apiUrl, ids, this.httpOptions);
  }
  
}
