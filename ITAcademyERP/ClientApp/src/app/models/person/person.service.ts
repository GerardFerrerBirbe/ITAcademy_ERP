import { HttpClient, HttpHeaderResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Person } from './person';

@Injectable({
  providedIn: 'root'
})
export class PersonService {

  private apiUrl = 'api/people';
  
  httpOptions = {
    headers: new HttpHeaders ({ 'Content-Type': 'application/json' })
  }

  constructor(private http: HttpClient) { }

  getPeople(): Observable<Person[]>{
    return this.http.get<Person[]>(this.apiUrl);
  }

}
