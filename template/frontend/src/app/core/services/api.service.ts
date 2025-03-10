import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpContext } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

/**
 * Interface for HTTP request options that extends Angular's options
 */
export interface RequestOptions {
  headers?: HttpHeaders | { [header: string]: string | string[] };
  params?: HttpParams | { [param: string]: string | number | boolean | ReadonlyArray<string | number | boolean> };
  context?: HttpContext;
  reportProgress?: boolean;
  withCredentials?: boolean;
  transferCache?: boolean | { includeHeaders?: string[] };
}

/**
 * Base API Service
 * 
 * Provides base methods for HTTP requests that can be used by all other services.
 * Extends functionality of Angular's HttpClient with consistent error handling.
 */
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  /**
   * API base URL from environment
   */
  private apiBaseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  /**
   * Performs an HTTP GET request
   * 
   * @param endpoint - The API endpoint (without the base URL)
   * @param options - Optional request configuration
   * @returns An observable with the response data
   */
  public get<T>(endpoint: string, options?: RequestOptions): Observable<T> {
    return this.http.get<T>(`${this.apiBaseUrl}${endpoint}`, options);
  }

  /**
   * Performs an HTTP POST request
   * 
   * @param endpoint - The API endpoint (without the base URL)
   * @param data - The data to send in the request body
   * @param options - Optional request configuration
   * @returns An observable with the response data
   */
  public post<T>(endpoint: string, data: any, options?: RequestOptions): Observable<T> {
    return this.http.post<T>(`${this.apiBaseUrl}${endpoint}`, data, options);
  }

  /**
   * Performs an HTTP PUT request
   * 
   * @param endpoint - The API endpoint (without the base URL)
   * @param data - The data to send in the request body
   * @param options - Optional request configuration
   * @returns An observable with the response data
   */
  public put<T>(endpoint: string, data: any, options?: RequestOptions): Observable<T> {
    return this.http.put<T>(`${this.apiBaseUrl}${endpoint}`, data, options);
  }

  /**
   * Performs an HTTP PATCH request
   * 
   * @param endpoint - The API endpoint (without the base URL)
   * @param data - The data to send in the request body
   * @param options - Optional request configuration
   * @returns An observable with the response data
   */
  public patch<T>(endpoint: string, data: any, options?: RequestOptions): Observable<T> {
    return this.http.patch<T>(`${this.apiBaseUrl}${endpoint}`, data, options);
  }

  /**
   * Performs an HTTP DELETE request
   * 
   * @param endpoint - The API endpoint (without the base URL)
   * @param options - Optional request configuration
   * @returns An observable with the response data
   */
  public delete<T>(endpoint: string, options?: RequestOptions): Observable<T> {
    return this.http.delete<T>(`${this.apiBaseUrl}${endpoint}`, options);
  }
} 