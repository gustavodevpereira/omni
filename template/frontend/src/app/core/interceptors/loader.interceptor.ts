import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { LoaderService } from '../services/loader.service';
import { LoggingService } from '../services/logging.service';

/**
 * Loader HTTP interceptor that manages the application's loading indicator.
 * 
 * @description
 * This interceptor automatically shows and hides the application's loading indicator
 * during HTTP requests. Specifically, it:
 * 
 * 1. Shows the loading indicator when HTTP requests start
 * 2. Hides the loading indicator when HTTP requests complete (success or error)
 * 3. Supports skipping the loader for certain requests via the x-skip-loader header
 * 
 * This creates a consistent user experience by providing visual feedback during
 * network operations without requiring individual components to manage loading states.
 * 
 * @example
 * // To skip the loader for specific requests:
 * httpClient.get('/api/data', {
 *   headers: new HttpHeaders().set('x-skip-loader', 'true')
 * });
 */
export const loaderInterceptor: HttpInterceptorFn = (req, next) => {
  // Services needed for loader functionality
  const loaderService = inject(LoaderService);
  const loggingService = inject(LoggingService);
  
  // Check if this request should skip the loading indicator
  const skipLoader = req.headers.has('x-skip-loader');
  
  if (skipLoader) {
    loggingService.logInfo(`Skipping loader for request to: ${req.url}`);
    // Continue without loader
    return next(req);
  }
  
  // Show loading indicator
  loggingService.logInfo(`Showing loader for request to: ${req.url}`);
  loaderService.show();
  
  // Continue with request and hide loader when done (success or error)
  return next(req).pipe(
    finalize(() => {
      loggingService.logInfo(`Hiding loader for request to: ${req.url}`);
      loaderService.hide();
    })
  );
}; 