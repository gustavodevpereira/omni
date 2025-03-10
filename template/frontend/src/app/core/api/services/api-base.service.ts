import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable, throwError, timer, finalize, of } from 'rxjs';
import { catchError, retry, timeout, map, retryWhen, tap, mergeMap } from 'rxjs/operators';
import { API_CONFIG } from '../config/api.config';
import { CommonResponses } from '../models/responses.model';
import { NotificationService } from '../../services/notification.service';
import { retry as rxjsRetry } from 'rxjs/operators';

/**
 * API Request Options
 */
export interface ApiRequestOptions {
  headers?: HttpHeaders | { [header: string]: string | string[] };
  params?: HttpParams | { [param: string]: string | number | boolean | ReadonlyArray<string | number | boolean> };
  reportProgress?: boolean;
  withCredentials?: boolean;
  responseType?: 'json';
  context?: HttpContext;
  skipErrorHandling?: boolean;
  skipLoading?: boolean;
  skipRetry?: boolean;
  customTimeout?: number;
}

/**
 * HTTP options for Angular HTTP client with specific observe value
 */
interface HttpOptions {
  headers?: HttpHeaders;
  observe: 'body';
  params?: HttpParams;
  reportProgress?: boolean;
  responseType?: 'json';
  withCredentials?: boolean;
  context?: HttpContext;
}

/**
 * API Base Service
 * 
 * Provides base HTTP methods for all API services.
 * Implements common functionality like error handling, retries, and timeout.
 */
@Injectable({
  providedIn: 'root'
})
export class ApiBaseService {
  /**
   * Base URL for API requests
   */
  protected baseUrl: string = API_CONFIG.BASE_URL;
  private retryMessageShown = false;

  constructor(
    protected http: HttpClient,
    private notificationService: NotificationService
  ) { }

  /**
   * Creates full API URL
   * 
   * @param endpoint - API endpoint path
   * @returns Full API URL
   */
  protected createUrl(endpoint: string): string {
    // Remove leading slash if present to avoid double slashes
    const formattedEndpoint = endpoint.startsWith('/') ? endpoint.slice(1) : endpoint;
    return `${this.baseUrl}${formattedEndpoint}`;
  }

  /**
   * Prepare request headers including custom headers
   * 
   * @param options - API request options
   * @returns HTTP headers
   */
  protected prepareHeaders(options?: ApiRequestOptions): HttpHeaders {
    let headers = new HttpHeaders();
    
    // Set default content type
    headers = headers.set(API_CONFIG.HEADERS.CONTENT_TYPE, API_CONFIG.CONTENT_TYPES.JSON);
    headers = headers.set(API_CONFIG.HEADERS.ACCEPT, API_CONFIG.CONTENT_TYPES.JSON);
    
    // Add custom headers if present
    if (options?.headers) {
      Object.entries(options.headers).forEach(([key, value]) => {
        if (typeof value === 'string') {
          headers = headers.set(key, value);
        } else if (Array.isArray(value)) {
          value.forEach(val => {
            headers = headers.append(key, val);
          });
        }
      });
    }
    
    // Add special flags for interceptors if needed
    if (options?.skipErrorHandling) {
      headers = headers.set('x-skip-error-handling', 'true');
    }
    
    if (options?.skipLoading) {
      headers = headers.set('x-skip-loading', 'true');
    }
    
    return headers;
  }

  /**
   * Creates HTTP parameters from object
   * 
   * @param params - Object with parameter values
   * @returns HTTP params
   */
  protected createHttpParams(params?: any): HttpParams {
    let httpParams = new HttpParams();
    
    if (params) {
      Object.entries(params).forEach(([key, value]) => {
        if (value !== null && value !== undefined) {
          httpParams = httpParams.set(key, value.toString());
        }
      });
    }
    
    return httpParams;
  }

  /**
   * Transforms API request options to Angular HTTP options
   * 
   * @param options - API request options
   * @returns HTTP options for Angular HTTP client
   */
  protected transformOptions(options?: ApiRequestOptions): HttpOptions {
    const baseOptions: HttpOptions = {
      observe: 'body'
    };
    
    if (!options) {
      return {
        ...baseOptions,
        headers: this.prepareHeaders()
      };
    }
    
    const { headers, params, responseType, withCredentials, reportProgress, context } = options;
    
    return {
      ...baseOptions,
      headers: this.prepareHeaders(options),
      params: params ? this.createHttpParams(params) : undefined,
      responseType,
      withCredentials,
      reportProgress,
      context
    };
  }

  /**
   * Base method for handling HTTP requests with appropriate error handling
   */
  private request<T>(
    method: string,
    endpoint: string,
    data: any = null,
    options?: ApiRequestOptions
  ): Observable<T> {
    // Create the full URL
    const url = this.createUrl(endpoint);
    
    // Transform API options to HttpOptions
    const httpOptions = this.transformOptions(options);
    
    // Apply custom timeout if specified
    const timeoutValue = options?.customTimeout || API_CONFIG.TIMEOUT;
    
    // Execute the appropriate HTTP method
    let request$: Observable<T>;
    
    switch(method) {
      case 'GET':
        request$ = this.http.get<T>(url, httpOptions);
        break;
      case 'POST':
        request$ = this.http.post<T>(url, data, httpOptions);
        break;
      case 'PUT':
        request$ = this.http.put<T>(url, data, httpOptions);
        break;
      case 'PATCH':
        request$ = this.http.patch<T>(url, data, httpOptions);
        break;
      case 'DELETE':
        request$ = this.http.delete<T>(url, httpOptions);
        break;
      default:
        return throwError(() => new Error(`Unsupported HTTP method: ${method}`));
    }
    
    // Apply timeout, error handling and return the observable
    return request$.pipe(
      timeout(timeoutValue),
      catchError(error => this.handleError(error) as Observable<never>)
    );
  }

  // Public API methods
  
  public get<T>(endpoint: string, options?: ApiRequestOptions): Observable<T> {
    return this.request<T>('GET', endpoint, null, options);
  }
  
  public post<T>(endpoint: string, data: any, options?: ApiRequestOptions): Observable<T> {
    return this.request<T>('POST', endpoint, data, options);
  }
  
  public put<T>(endpoint: string, data: any, options?: ApiRequestOptions): Observable<T> {
    return this.request<T>('PUT', endpoint, data, options);
  }
  
  public patch<T>(endpoint: string, data: any, options?: ApiRequestOptions): Observable<T> {
    return this.request<T>('PATCH', endpoint, data, options);
  }
  
  public delete<T>(endpoint: string, options?: ApiRequestOptions): Observable<T> {
    return this.request<T>('DELETE', endpoint, null, options);
  }
  
  public getWithFullResponse<T>(endpoint: string, options?: ApiRequestOptions): Observable<HttpResponse<T>> {
    const url = this.createUrl(endpoint);
    const httpOptions = this.transformOptions(options);
    httpOptions.observe = 'response' as any;
    
    return this.http.get<HttpResponse<T>>(url, httpOptions).pipe(
      timeout(options?.customTimeout || API_CONFIG.TIMEOUT),
      catchError(error => this.handleError(error) as Observable<never>)
    );
  }
  
  /**
   * Handle HTTP error responses
   */
  protected handleError(error: any): Observable<never> {
    console.error('API error:', error);
    
    let errorMessage = 'An error occurred';
    let errorCode = 'UNKNOWN_ERROR';
    let errorDetails = null;
    
    if (error.error) {
      errorMessage = error.error.message || errorMessage;
      errorCode = error.error.code || errorCode;
      errorDetails = error.error.details || errorDetails;
    }
    
    return throwError(() => ({
      message: errorMessage,
      code: errorCode,
      status: error.status,
      details: errorDetails,
      originalError: error
    }));
  }
} 