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
import { Router } from '@angular/router';
import { NotificationService } from '../services/notification.service';

/**
 * Error Interceptor
 * 
 * Intercepts HTTP responses and handles error responses,
 * providing consistent error handling across the application.
 * Uses the NotificationService to display error messages to the user.
 */
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private router: Router,
    private notificationService: NotificationService
  ) {}

  /**
   * Intercepts HTTP responses and handles errors appropriately
   * 
   * @param request - The original HTTP request
   * @param next - The HTTP handler for the request pipeline
   * @returns An observable of the HTTP event stream
   */
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Skip error handling for specific requests if needed
    if (request.headers.has('x-skip-error-handling')) {
      const newRequest = request.clone({
        headers: request.headers.delete('x-skip-error-handling')
      });
      return next.handle(newRequest);
    }
    
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'An unknown error occurred';
        
        if (error.error instanceof ErrorEvent) {
          // Client-side error
          errorMessage = `Error: ${error.error.message}`;
          console.error('Client error:', error.error.message);
        } else {
          // Server-side error
          console.error('Server error:', error);
          
          // Handle specific HTTP status codes
          switch (error.status) {
            case 0:
              errorMessage = 'Cannot connect to the server. Please check your internet connection.';
              break;
            case 400:
              errorMessage = this.handleBadRequest(error);
              break;
            case 401: // Unauthorized
              errorMessage = 'Your session has expired. Please log in again.';
              localStorage.removeItem('auth_token');
              this.router.navigate(['/auth/login']);
              break;
            case 403: // Forbidden
              errorMessage = 'You do not have permission to access this resource.';
              break;
            case 404: // Not Found
              errorMessage = 'The requested resource was not found.';
              break;
            case 500: // Server Error
              errorMessage = 'A server error occurred. Please try again later.';
              break;
            default:
              errorMessage = `Error ${error.status}: ${error.statusText || 'Unknown error'}`;
              break;
          }
        }
        
        // Show the error message to the user
        this.notificationService.error(errorMessage);
        
        // Re-throw the error for components to handle if needed
        return throwError(() => new Error(errorMessage));
      })
    );
  }

  /**
   * Handles Bad Request (400) errors, which often contain validation errors
   * 
   * @param error - The HTTP error response
   * @returns A formatted error message
   */
  private handleBadRequest(error: HttpErrorResponse): string {
    // Check if the error contains validation errors
    if (error.error && error.error.errors && typeof error.error.errors === 'object') {
      // Collect all validation errors
      const validationErrors: string[] = [];
      Object.entries(error.error.errors).forEach(([field, messages]) => {
        if (Array.isArray(messages)) {
          messages.forEach(message => {
            validationErrors.push(`${field}: ${message}`);
          });
        } else {
          validationErrors.push(`${field}: ${messages}`);
        }
      });
      
      // Return a single string with validation errors
      if (validationErrors.length > 0) {
        if (validationErrors.length > 2) {
          return `${validationErrors[0]}, ${validationErrors[1]} and ${validationErrors.length - 2} more errors`;
        } else {
          return validationErrors.join(', ');
        }
      }
    }
    
    // If no validation errors or other structure, use the error message or a default
    return error.error?.message || error.message || 'Invalid request';
  }
} 