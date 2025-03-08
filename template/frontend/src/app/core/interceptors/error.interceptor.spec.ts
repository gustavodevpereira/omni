import { TestBed } from '@angular/core/testing';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorInterceptor } from './error.interceptor';
import { LoggingService } from '../services/logging.service';
import { runInInjectionContext } from '@angular/core';

/**
 * Test suite for Error Interceptor
 * 
 * @description
 * Verifies the error interceptor properly handles:
 * - Successful responses passing through without modification
 * - Network errors (status 0) being properly captured and logged
 * - Other error types being passed through for handling elsewhere
 */
describe('errorInterceptor', () => {
  let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;
  let loggingService: jasmine.SpyObj<LoggingService>;
  
  beforeEach(() => {
    // Create spy services
    loggingService = jasmine.createSpyObj('LoggingService', ['logError']);
    
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([errorInterceptor])),
        provideHttpClientTesting(),
        { provide: LoggingService, useValue: loggingService }
      ]
    });
    
    // Inject the http service and test controller
    httpClient = TestBed.inject(HttpClient);
    httpTestingController = TestBed.inject(HttpTestingController);
  });
  
  afterEach(() => {
    // Verify that no requests are outstanding
    httpTestingController.verify();
  });
  
  /**
   * Test that the interceptor exists and can be imported
   */
  it('should be created', () => {
    expect(errorInterceptor).toBeTruthy();
  });
  
  /**
   * Test that successful responses pass through without modification
   */
  it('should pass through successful responses', () => {
    // Make HTTP GET request
    httpClient.get('/api/test').subscribe(response => {
      expect(response).toBeTruthy();
    });
    
    // The following request should have been made
    const req = httpTestingController.expectOne('/api/test');
    
    // Respond with mock data
    req.flush({ data: 'test' });
    
    // LoggingService should not be called for successful responses
    expect(loggingService.logError).not.toHaveBeenCalled();
  });
  
  /**
   * Test that network errors (status 0) are properly handled and logged
   */
  it('should handle network errors (status 0)', () => {
    // Make HTTP GET request
    httpClient.get('/api/test').subscribe({
      next: () => fail('Should have failed with a network error'),
      error: (error: HttpErrorResponse) => {
        // Network error should be status 0
        expect(error.status).toBe(0);
      }
    });
    
    // The following request should have been made
    const req = httpTestingController.expectOne('/api/test');
    
    // Respond with a network error
    req.error(new ErrorEvent('Network error'), { status: 0 });
    
    // Verify logging service was called with appropriate message
    expect(loggingService.logError).toHaveBeenCalledWith(
      'Network error or CORS issue detected',
      jasmine.any(HttpErrorResponse)
    );
  });
  
  /**
   * Test that other error types pass through for handling elsewhere
   */
  it('should pass through other error types', () => {
    // Make HTTP GET request
    httpClient.get('/api/test').subscribe({
      next: () => fail('Should have failed with a 404 error'),
      error: (error: HttpErrorResponse) => {
        // Should be a 404 error
        expect(error.status).toBe(404);
      }
    });
    
    // The following request should have been made
    const req = httpTestingController.expectOne('/api/test');
    
    // Respond with a 404 error
    req.flush('Not Found', { status: 404, statusText: 'Not Found' });
    
    // Verify logging service was not called for non-network errors
    expect(loggingService.logError).not.toHaveBeenCalled();
  });
});
