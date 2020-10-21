import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Client } from './client';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  private apiUrl = 'api/clients';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }

  constructor(private http: HttpClient) { }

  getClients(): Observable<Client[]>{
    return this.http.get<Client[]>(this.apiUrl);
  }

  getClient(id: string): Observable<Client> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<Client>(url);
  }

  updateClient(client: Client): Observable<Client> {
    return this.http.put<Client>(this.apiUrl + "/" + client.id, client, this.httpOptions);
  }

  addClient(client: Client): Observable<Client> {
    return this.http.post<Client>(this.apiUrl, client, this.httpOptions);
  }

  deleteClient(client: Client | string): Observable<Client> {
    const id = typeof client === 'string' ? client : client.id;
    const url = `${this.apiUrl}/${id}`;

    return this.http.delete<Client>(url, this.httpOptions);
  }
}