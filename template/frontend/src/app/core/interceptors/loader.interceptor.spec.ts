import { TestBed } from '@angular/core/testing';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { loaderInterceptor } from './loader.interceptor';
import { LoaderService } from '../services/loader.service';
import { LoggingService } from '../services/logging.service';

/**
 * Test suite for Loader Interceptor
 * 
 * @description
 * Verifies the loader interceptor properly:
 * - Shows loading indicator for normal HTTP requests
 * - Hides loading indicator when requests complete (success or error)
 * - Skips the loading indicator for requests with skip-loader header
 * - Logs appropriate information during the loading process
 */
describe('loaderInterceptor', () => {
  let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;
  let loaderService: jasmine.SpyObj<LoaderService>;
  let loggingService: jasmine.SpyObj<LoggingService>;
  
  beforeEach(() => {
    // Create spy services
    loaderService = jasmine.createSpyObj('LoaderService', ['show', 'hide']);
    loggingService = jasmine.createSpyObj('LoggingService', ['logInfo']);
    
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([loaderInterceptor])),
        provideHttpClientTesting(),
        { provide: LoaderService, useValue: loaderService },
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
   * Test that the interceptor shows loader for normal requests and hides on completion
   */
  it('should show loader for normal requests and hide on completion', () => {
    // Make an HTTP request
    httpClient.get('/api/test').subscribe();
    
    // The following request should have been made
    const req = httpTestingController.expectOne('/api/test');
    
    // Verify loader was shown and logging occurred
    expect(loaderService.show).toHaveBeenCalled();
    expect(loggingService.logInfo).toHaveBeenCalledWith(jasmine.stringContaining('Showing loader'));
    
    // Respond with mock data
    req.flush({ data: 'test' });
    
    // Verify loader was hidden and logging occurred
    expect(loaderService.hide).toHaveBeenCalled();
    expect(loggingService.logInfo).toHaveBeenCalledWith(jasmine.stringContaining('Hiding loader'));
  });
  
  /**
   * Test that the interceptor hides loader even when request fails
   */
  it('should hide loader even when request fails', () => {
    // Make an HTTP request that will fail
    httpClient.get('/api/test').subscribe({
      next: () => fail('Should have failed with error'),
      error: () => {}
    });
    
    // The following request should have been made
    const req = httpTestingController.expectOne('/api/test');
    
    // Verify loader was shown
    expect(loaderService.show).toHaveBeenCalled();
    
    // Respond with an error
    req.error(new ErrorEvent('Test error'));
    
    // Verify loader was hidden even after error
    expect(loaderService.hide).toHaveBeenCalled();
    expect(loggingService.logInfo).toHaveBeenCalledWith(jasmine.stringContaining('Hiding loader'));
  });
  
  /**
   * Test that the interceptor skips loader for requests with x-skip-loader header
   */
  it('should skip loader for requests with x-skip-loader header', () => {
    // Make an HTTP request with skip-loader header
    httpClient.get('/api/test', {
      headers: new HttpHeaders().set('x-skip-loader', 'true')
    }).subscribe();
    
    // The following request should have been made
    const req = httpTestingController.expectOne('/api/test');
    
    // Verify loader was NOT shown
    expect(loaderService.show).not.toHaveBeenCalled();
    expect(loggingService.logInfo).toHaveBeenCalledWith(jasmine.stringContaining('Skipping loader'));
    
    // Respond with mock data
    req.flush({ data: 'test' });
    
    // Verify loader was NOT hidden
    expect(loaderService.hide).not.toHaveBeenCalled();
  });
});
