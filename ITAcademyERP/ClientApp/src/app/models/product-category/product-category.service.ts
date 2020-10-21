import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { ProductCategory } from './product-category';

@Injectable({
  providedIn: 'root'
})
export class ProductCategoryService {

  private apiUrl = 'api/productCategories';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  }

  constructor(private http: HttpClient) { }

  getProductCategories(): Observable<ProductCategory[]>{
    return this.http.get<ProductCategory[]>(this.apiUrl + "/generic/");
  }

  getProductCategory(id: string): Observable<ProductCategory> {
    const url = `${this.apiUrl}/generic/${id}`;
    return this.http.get<ProductCategory>(url);
  }

  updateProductCategory(productCategory: ProductCategory): Observable<ProductCategory> {
    return this.http.put<ProductCategory>(this.apiUrl + "/generic/" + productCategory.id, productCategory, this.httpOptions);
  }

  addProductCategory(productCategory: ProductCategory): Observable<ProductCategory> {
    return this.http.post<ProductCategory>(this.apiUrl + "/generic/", productCategory, this.httpOptions);
  }

  deleteProductCategory(productCategory: ProductCategory | string): Observable<ProductCategory> {
    const id = typeof productCategory === 'string' ? productCategory : productCategory.id;
    const url = `${this.apiUrl}/generic/${id}`;

    return this.http.delete<ProductCategory>(url, this.httpOptions);
  }
}
