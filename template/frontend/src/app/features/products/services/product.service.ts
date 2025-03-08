import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { Product } from '../../../shared/models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private endpoint = 'products';

  constructor(private apiService: ApiService) { }

  getProducts(params?: any): Observable<{ items: Product[], totalCount: number }> {
    return this.apiService.get<{ items: Product[], totalCount: number }>(this.endpoint, params);
  }

  getProduct(id: string): Observable<Product> {
    return this.apiService.get<Product>(`${this.endpoint}/${id}`);
  }

  createProduct(product: Partial<Product>): Observable<Product> {
    return this.apiService.post<Product>(this.endpoint, product);
  }

  updateProduct(id: string, product: Partial<Product>): Observable<Product> {
    return this.apiService.put<Product>(`${this.endpoint}/${id}`, product);
  }

  deleteProduct(id: string): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
} 