import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

/**
 * Authentication Interceptor
 * 
 * Intercepts all HTTP requests and adds the bearer token from localStorage
 * to the Authorization header if available.
 */
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  /**
   * Intercepts HTTP requests and adds the Authorization header with bearer token
   * 
   * @param request - The original HTTP request
   * @param next - The HTTP handler for the request pipeline
   * @returns An observable of the HTTP event stream
   */
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Skip authentication for certain endpoints
    if (this.shouldSkipAuth(request)) {
      return next.handle(request);
    }
    
    // Get token from localStorage
    const token = localStorage.getItem('auth_token');
    
    // If token exists, clone the request and add the authorization header
    if (token) {
      request = this.addToken(request, token);
    }
    
    // Process the request
    return next.handle(request).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          // Handle 401 errors (Unauthorized) - log out the user
          this.authService.logout();
        }
        
        return throwError(() => error);
      })
    );
  }

  /**
   * Determines if authentication should be skipped for a request
   * 
   * @param request - HTTP request
   * @returns True if auth should be skipped
   */
  private shouldSkipAuth(request: HttpRequest<any>): boolean {
    // Skip if request explicitly asks to skip auth
    if (request.headers.has('x-skip-auth')) {
      return true;
    }
    
    // Skip auth for login and register endpoints
    const url = request.url.toLowerCase();
    return (
      url.includes('/auth/login') || 
      url.includes('/auth/register') ||
      url.includes('/auth/forgot-password') ||
      url.includes('/auth/reset-password')
    );
  }

  /**
   * Add authorization token to a request
   * 
   * @param request - HTTP request
   * @param token - Authorization token
   * @returns Request with token
   */
  private addToken(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
} 