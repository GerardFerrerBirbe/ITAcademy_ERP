import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { ProductCategory } from '../models/product-category/product-category';

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
    return this.http.get<ProductCategory[]>(this.apiUrl);
  }

  getProductCategory(id: string): Observable<ProductCategory> {
    let params = new HttpParams().set('includeProducts', "true");
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<ProductCategory>(url, {params: params});
  }

  updateProductCategory(productCategory: ProductCategory): Observable<ProductCategory> {
    return this.http.put<ProductCategory>(this.apiUrl + "/" + productCategory.id.toString(), productCategory, this.httpOptions);
  }

  addProductCategory(productCategory: ProductCategory): Observable<ProductCategory> {
    return this.http.post<ProductCategory>(this.apiUrl, productCategory, this.httpOptions);
  }

  deleteProductCategory(productCategory: ProductCategory | number): Observable<ProductCategory> {
    const id = typeof productCategory === 'number' ? productCategory : productCategory.id;
    const url = `${this.apiUrl}/${id}`;

    return this.http.delete<ProductCategory>(url, this.httpOptions);
  }
}