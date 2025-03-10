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
import { CommonResponses } from '../api/models/responses.model';

/**
 * Error Interceptor
 * 
 * Intercepts HTTP responses and handles error responses,
 * providing consistent error handling across the application.
 */
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private router: Router,
    private notificationService: NotificationService
  ) {}

  /**
   * Intercepts HTTP responses and handles errors appropriately
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
        // Client-side or network error
        if (error.error instanceof ErrorEvent) {
          const errorMessage = `Error: ${error.error.message}`;
          console.error('Client error:', error.error.message);
          this.notificationService.error(errorMessage);
          return throwError(() => new Error(errorMessage));
        }
        
        // Handle 401 (Unauthorized) - Always redirect to login
        if (error.status === 401) {
          localStorage.removeItem('auth_token');
          this.router.navigate(['/auth/login']);
          const errorMessage = 'Your session has expired. Please log in again.';
          this.notificationService.error(errorMessage);
          return throwError(() => new Error(errorMessage));
        }
        
        // Try to parse as API error response
        try {
          const apiError = error.error as CommonResponses.ApiResponse<unknown>;
          
          // Check if the response follows our API error format
          if (apiError && typeof apiError.success === 'boolean') {
            // Get the main error message
            let errorMessage = apiError.message || 'An error occurred';
            
            // If there are detailed validation errors, add them
            if (apiError.error?.validationErrors) {
              console.error('API Validation Errors:', apiError.error.validationErrors);
              
              // Extract detailed errors from validationErrors
              const detailedErrors: string[] = [];
              Object.values(apiError.error.validationErrors).forEach(errors => {
                if (Array.isArray(errors)) {
                  detailedErrors.push(...errors);
                }
              });
              
              // Add up to 3 detailed errors to the message
              if (detailedErrors.length > 0) {
                if (detailedErrors.length <= 3) {
                  errorMessage += ': ' + detailedErrors.join(', ');
                } else {
                  errorMessage += `: ${detailedErrors[0]}, ${detailedErrors[1]} and ${detailedErrors.length - 2} more errors`;
                }
              }
            }
            
            this.notificationService.error(errorMessage);
            return throwError(() => new Error(errorMessage));
          }
        } catch (e) {
          console.error('Error parsing API error response:', e);
        }
        
        // Default fallback error handling
        const fallbackMessage = `Error ${error.status || 0}: ${error.statusText || 'Could not connect to the server. Please try again later.'}`;
        
        console.error('Server error with non-standard format:', error);
        this.notificationService.error(fallbackMessage);
        return throwError(() => new Error(fallbackMessage));
      })
    );
  }
}