import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Person } from '../models/person/person';


@Injectable({
  providedIn: 'root'
})
export class PersonService {

  private apiUrl = 'api/people';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }

  constructor(private http: HttpClient) { }

  getPeople(): Observable<Person[]>{
    var output = this.http.get<Person[]>(this.apiUrl);
    return output;
  }

  getPerson(id: string): Observable<Person> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<Person>(url)
  }

  updatePerson(person: Person): Observable<Person> {
    return this.http.put<Person>(this.apiUrl + "/" + person.id.toString(), person, this.httpOptions);
  }

  addPerson(person: Person): Observable<Person> {
    return this.http.post<Person>(this.apiUrl, person, this.httpOptions);
  }

  deletePerson(person: Person | number): Observable<Person> {
    const id = typeof person === 'number' ? person : person.id;
    const url = `${this.apiUrl}/${id}`;

    return this.http.delete<Person>(url, this.httpOptions);
  }
}
