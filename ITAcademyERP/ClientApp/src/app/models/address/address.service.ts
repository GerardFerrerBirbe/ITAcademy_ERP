import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddressService {

  private apiUrl = 'api/Addresses';

httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json'})
}

  constructor(private http: HttpClient) { }

  deleteAddresses(ids: number[]): Observable<void>{
    return this.http.post<void>(this.apiUrl, ids, this.httpOptions);
  }
}
