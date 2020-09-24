import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Role } from '../models/role/role';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  private apiUrl = 'api/roles';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }

  constructor(private http: HttpClient) { }

  getRoles(): Observable<Role[]>{
    return this.http.get<Role[]>(this.apiUrl);
  }

  getRole(id: string): Observable<Role> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<Role>(url);
  }

  updateRole(role: Role): Observable<Role> {
    return this.http.put<Role>(this.apiUrl + "/" + role.id.toString(), role, this.httpOptions);
  }

  addRole(role: Role): Observable<Role> {
    return this.http.post<Role>(this.apiUrl, role, this.httpOptions);
  }

  deleteRole(role: Role | number): Observable<Role> {
    const id = typeof role === 'number' ? role : role.id;
    const url = `${this.apiUrl}/${id}`;

    return this.http.delete<Role>(url, this.httpOptions);
  }
}
