import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { inject } from '@angular/core';
import { LoggingService } from '../services/logging.service';

/**
 * Error HTTP interceptor that provides centralized error handling for HTTP requests.
 * 
 * @description
 * This interceptor captures HTTP errors before they reach components, specifically handling:
 * - Network errors and connection issues (status 0)
 * - Logging appropriate error information using LoggingService
 * - Re-throwing errors to be handled by the global error handler
 * 
 * The interceptor is a critical part of the application's error handling infrastructure,
 * ensuring that all HTTP errors are properly logged and can be monitored.
 * 
 * @example
 * // Register in app.config.ts
 * export const appConfig: ApplicationConfig = {
 *   providers: [
 *     provideHttpClient(
 *       withInterceptors([errorInterceptor])
 *     )
 *   ]
 * };
 */
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  // Use a synchronous technique for getting the service to avoid injection context errors
  const loggingService = inject(LoggingService);
  
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Special handling for network errors (status 0)
      if (error.status === 0) {
        loggingService.logError('Network error or CORS issue detected', error);
      }
      
      // Re-throw error to be caught by the global handler
      return throwError(() => error);
    })
  );
}; 