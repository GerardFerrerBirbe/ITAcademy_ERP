import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Address } from './address';

@Injectable({
  providedIn: 'root'
})
export class AddressService {

  private apiUrl = 'api/Addresses';

httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json'})
}

  constructor(private http: HttpClient) { }

  deleteAddresses(addresses: string[]): Observable<void>{
    return this.http.post<void>(this.apiUrl, addresses, this.httpOptions);
  }
}
