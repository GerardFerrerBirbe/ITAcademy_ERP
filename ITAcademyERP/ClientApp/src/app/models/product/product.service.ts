import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Product } from './product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiUrl = 'api/products';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }

  constructor(private http: HttpClient) { }

  getProducts(): Observable<Product[]>{
    return this.http.get<Product[]>(this.apiUrl);
  }

  getProduct(id: string): Observable<Product> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<Product>(url);
  }

  updateProduct(product: Product): Observable<Product> {
    return this.http.put<Product>(this.apiUrl + "/" + product.id.toString(), product, this.httpOptions);
  }

  addProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product, this.httpOptions);
  }

  deleteProduct(product: Product | number): Observable<Product> {
    const id = typeof product === 'number' ? product : product.id;
    const url = `${this.apiUrl}/generic/${id}`;

    return this.http.delete<Product>(url, this.httpOptions);
  }
}
