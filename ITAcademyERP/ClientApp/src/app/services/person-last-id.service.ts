import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PersonLastIdService {

  private apiUrl = 'api/peopleLastId';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }

  constructor(private http: HttpClient) { }

  getPeopleLastId(): Observable<number>{
    var output = this.http.get<number>(this.apiUrl);
    return output;
  }
}
