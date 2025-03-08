import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoggingService } from './logging.service';

/**
 * Core API service that handles all HTTP communication with the backend.
 * 
 * @description
 * This service provides a centralized way to make HTTP requests to the API.
 * It offers typed generic methods for GET, POST, PUT, and DELETE operations,
 * handling URL construction, parameter formatting, and request execution.
 * 
 * The service abstracts the details of HTTP communication, allowing components
 * and other services to focus on business logic rather than HTTP implementation.
 * 
 * @example
 * // Fetching a product by ID
 * this.apiService.get<Product>(`products/${id}`).subscribe(
 *   product => this.handleProduct(product)
 * );
 * 
 * // Creating a new user
 * this.apiService.post<ApiResponse<User>>('users', userData).subscribe(
 *   response => this.handleNewUser(response)
 * );
 */
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  /** Base API URL from environment configuration */
  private apiUrl = environment.apiUrl;

  /**
   * Creates an instance of ApiService.
   * 
   * @param http - Angular's HttpClient for making HTTP requests
   * @param loggingService - Service for application logging
   */
  constructor(
    private http: HttpClient,
    private loggingService: LoggingService
  ) {
    this.loggingService.logInfo('API Service initialized', { apiUrl: this.apiUrl });
  }

  /**
   * Performs an HTTP GET request to retrieve data from the API.
   * 
   * @param endpoint - API endpoint path (without base URL)
   * @param params - Optional query parameters as key-value pairs
   * @returns Observable of the response data with type T
   * 
   * @example
   * // Get all products
   * apiService.get<Product[]>('products')
   * 
   * // Get filtered products
   * apiService.get<Product[]>('products', { category: 'electronics', sort: 'price' })
   */
  get<T>(endpoint: string, params?: Record<string, any>): Observable<T> {
    let httpParams = new HttpParams();
    
    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key] !== null && params[key] !== undefined) {
          httpParams = httpParams.set(key, params[key]);
        }
      });
    }
    
    const url = `${this.apiUrl}/${endpoint}`;
    this.loggingService.logInfo('Making GET request', { url, params });
    
    return this.http.get<T>(url, { params: httpParams });
  }

  /**
   * Performs an HTTP POST request to create a new resource or submit data.
   * 
   * @param endpoint - API endpoint path (without base URL)
   * @param data - Request body data to send
   * @returns Observable of the response data with type T
   * 
   * @example
   * // Create a new product
   * apiService.post<Product>('products', {
   *   name: 'New product',
   *   price: 99.99
   * })
   */
  post<T>(endpoint: string, data: any): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;
    this.loggingService.logInfo('Making POST request', { url, data });
    
    return this.http.post<T>(url, data);
  }

  /**
   * Performs an HTTP PUT request to update an existing resource.
   * 
   * @param endpoint - API endpoint path (without base URL)
   * @param data - Request body data to send
   * @returns Observable of the response data with type T
   * 
   * @example
   * // Update a product
   * apiService.put<Product>(`products/${id}`, {
   *   name: 'Updated product name',
   *   price: 129.99
   * })
   */
  put<T>(endpoint: string, data: any): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;
    this.loggingService.logInfo('Making PUT request', { url, data });
    
    return this.http.put<T>(url, data);
  }

  /**
   * Performs an HTTP DELETE request to remove a resource.
   * 
   * @param endpoint - API endpoint path (without base URL)
   * @returns Observable of the response data with type T
   * 
   * @example
   * // Delete a product
   * apiService.delete<ApiResponse<void>>(`products/${id}`)
   */
  delete<T>(endpoint: string): Observable<T> {
    const url = `${this.apiUrl}/${endpoint}`;
    this.loggingService.logInfo('Making DELETE request', { url });
    
    return this.http.delete<T>(url);
  }
}
