import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { LoadingService } from '../services/loading.service';

/**
 * Loading Interceptor
 * 
 * Intercepts all HTTP requests and shows a loading indicator.
 * Uses the LoadingService to manage the loading state.
 */
@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private loadingService: LoadingService) {}

  /**
   * Intercepts HTTP requests and shows a loading indicator
   * 
   * @param request - The HTTP request
   * @param next - The HTTP handler
   * @returns An observable of the HTTP event
   */
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Skip loading indicator for specific requests if needed
    // For example, you might not want to show loading for background operations
    if (request.headers.has('x-skip-loading')) {
      const newRequest = request.clone({
        headers: request.headers.delete('x-skip-loading')
      });
      return next.handle(newRequest);
    }

    // Start the loading indicator
    this.loadingService.startLoading();

    // Process the request and handle the response or error
    return next.handle(request).pipe(
      finalize(() => {
        // Stop the loading indicator regardless of success or error
        this.loadingService.stopLoading();
      })
    );
  }
} 