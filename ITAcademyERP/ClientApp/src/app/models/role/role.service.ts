import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Role } from './role';
import { RoleUser } from './roleUser';

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

  getUsers(): Observable<RoleUser[]>{
    return this.http.get<RoleUser[]>(this.apiUrl + "/RoleUsers");
  }

  getRole(id: string): Observable<Role> {
    let params = new HttpParams().set('includeUsers', "true");
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<Role>(url, {params: params});
  } 

  updateRole(role: Role): Observable<Role> {
    return this.http.put<Role>(this.apiUrl + "/" + role.id, role, this.httpOptions);
  }

  addRole(role: Role): Observable<Role> {
    return this.http.post<Role>(this.apiUrl, role, this.httpOptions);
  }

  deleteRole(role: Role | string): Observable<Role> {
    const id = typeof role === 'string' ? role : role.id;
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete<Role>(url, this.httpOptions);
  }

  updateRoleUser(roleUser: RoleUser, addOrRemove: string): Observable<RoleUser>{
    let params = new HttpParams().set('addOrRemove', addOrRemove);
    return this.http.put<RoleUser>(this.apiUrl, roleUser, {params: params});
  }
}
