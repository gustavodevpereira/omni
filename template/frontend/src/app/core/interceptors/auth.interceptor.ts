import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';
import { LoggingService } from '../services/logging.service';

/**
 * Authentication HTTP interceptor for handling authorization and auth-related errors.
 * 
 * @description
 * This interceptor performs several critical authentication tasks:
 * 
 * 1. Adds Authorization header with JWT token to outgoing requests when a token exists
 * 2. Processes authentication-related error responses (401, 403, 500)
 * 3. Handles session expiration by redirecting to login
 * 4. Provides appropriate user notifications for different auth scenarios
 * 5. Logs authentication-related events for monitoring and debugging
 * 
 * The interceptor is a key component in maintaining the application's security boundary
 * and providing a consistent user experience for authentication-related scenarios.
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Access required services
  const authService = inject(AuthService);
  const router = inject(Router);
  const notificationService = inject(NotificationService);
  const loggingService = inject(LoggingService);

  // Get authentication token from storage
  const token = localStorage.getItem('auth_token');
  
  loggingService.logInfo(`Processing request to: ${req.url}`);
  
  // If token exists, add Authorization header to the request
  if (token) {
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    
    loggingService.logInfo('Authorization header added to request');
    
    // Process the authenticated request and handle auth-related errors
    return next(authReq).pipe(
      catchError(error => {
        // Handle authentication errors
        if (error.status === 401) {
          // Only redirect to login if not already on login page
          if (!router.url.includes('/auth/login')) {
            loggingService.logWarning('Received 401 Unauthorized response, logging out user');
            authService.logout();
            router.navigate(['/auth/login']);
            notificationService.error('Your session has expired. Please log in again.');
          }
        } else if (error.status === 403) {
          loggingService.logWarning(`Access forbidden to resource: ${req.url}`);
          notificationService.error('You do not have permission to perform this action.');
        } else if (error.status === 500) {
          loggingService.logError('Server error occurred', error);
          notificationService.error('A server error occurred. Please try again later.');
        }
        
        // Re-throw the error for other interceptors or error handlers
        return throwError(() => error);
      })
    );
  }

  // If no token, process the request without authentication
  loggingService.logInfo('No authentication token available, proceeding without Authorization header');
  return next(req).pipe(
    catchError(error => {
      if (error.status === 500) {
        loggingService.logError('Server error occurred', error);
        notificationService.error('A server error occurred. Please try again later.');
      }
      return throwError(() => error);
    })
  );
};
