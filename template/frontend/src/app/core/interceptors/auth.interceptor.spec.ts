import { TestBed } from '@angular/core/testing';
import { HttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './auth.interceptor';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { NotificationService } from '../services/notification.service';
import { LoggingService } from '../services/logging.service';

/**
 * Test suite for Authentication Interceptor
 * 
 * @description
 * Verifies the authentication interceptor properly:
 * - Adds Authorization header when token is available
 * - Handles 401 Unauthorized responses (logout and redirect)
 * - Handles 403 Forbidden responses with appropriate notifications
 * - Handles 500 Server Error responses
 * - Logs appropriate information during authentication processes
 */
describe('authInterceptor', () => {
  let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;
  let notificationService: jasmine.SpyObj<NotificationService>;
  let loggingService: jasmine.SpyObj<LoggingService>;
  let localStorageSpy: jasmine.Spy;
  
  beforeEach(() => {
    // Create spy services
    authService = jasmine.createSpyObj('AuthService', ['logout']);
    router = jasmine.createSpyObj('Router', ['navigate'], { url: '/dashboard' });
    notificationService = jasmine.createSpyObj('NotificationService', ['error']);
    loggingService = jasmine.createSpyObj('LoggingService', ['logInfo', 'logWarning', 'logError']);
    
    // Spy on localStorage
    localStorageSpy = spyOn(localStorage, 'getItem');
    
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
        { provide: AuthService, useValue: authService },
        { provide: Router, useValue: router },
        { provide: NotificationService, useValue: notificationService },
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
   * Test that the interceptor adds Authorization header when token exists
   */
  it('should add Authorization header when token exists', () => {
    // Configure localStorage to return a token
    const token = 'test-token';
    localStorageSpy.and.returnValue(token);
    
    // Make an HTTP request
    httpClient.get('/api/test').subscribe();
    
    // The following request should have been made
    const req = httpTestingController.expectOne('/api/test');
    
    // Verify request headers
    expect(req.request.headers.has('Authorization')).toBeTrue();
    expect(req.request.headers.get('Authorization')).toBe(`Bearer ${token}`);
    expect(loggingService.logInfo).toHaveBeenCalled();
    
    // Respond with mock data
    req.flush({ data: 'test' });
  });
  
  /**
   * Test that the interceptor does not add Authorization header when no token exists
   */
  it('should not add Authorization header when no token exists', () => {
    // Configure localStorage to return null (no token)
    localStorageSpy.and.returnValue(null);
    
    // Make an HTTP request
    httpClient.get('/api/test').subscribe();
    
    // The following request should have been made
    const req = httpTestingController.expectOne('/api/test');
    
    // Verify request headers
    expect(req.request.headers.has('Authorization')).toBeFalse();
    
    // Respond with mock data
    req.flush({ data: 'test' });
  });
  
  it('should handle 401 errors and log the user out', () => {
    // Configure localStorage to return a token
    localStorageSpy.and.returnValue('test-token');
    
    // Make an HTTP request
    httpClient.get('/api/test').subscribe({
      next: () => fail('Should have failed with 401 error'),
      error: () => {
        // Verify service calls
        expect(authService.logout).toHaveBeenCalled();
        expect(router.navigate).toHaveBeenCalledWith(['/auth/login']);
        expect(notificationService.error).toHaveBeenCalledWith(
          'Your session has expired. Please log in again.'
        );
      }
    });
    
    // The following request should have been made
    const req = httpTestingController.expectOne('/api/test');
    
    // Respond with a 401 error
    req.flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });
  });
  
  it('should not logout on 401 if already on login page', () => {
    // Configure localStorage to return a token
    localStorageSpy.and.returnValue('test-token');
    
    // Change router url to login page
    Object.defineProperty(router, 'url', { value: '/auth/login' });
    
    // Make an HTTP request
    httpClient.get('/api/test').subscribe({
      next: () => fail('Should have failed with 401 error'),
      error: () => {
        // Verify service calls were NOT made
        expect(authService.logout).not.toHaveBeenCalled();
        expect(router.navigate).not.toHaveBeenCalled();
      }
    });
    
    // The following request should have been made
    const req = httpTestingController.expectOne('/api/test');
    
    // Respond with a 401 error
    req.flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });
  });
  
  it('should show appropriate notification for 403 errors', () => {
    // Configure localStorage to return a token
    localStorageSpy.and.returnValue('test-token');
    
    // Make an HTTP request
    httpClient.get('/api/test').subscribe({
      next: () => fail('Should have failed with 403 error'),
      error: () => {
        // Verify notification
        expect(notificationService.error).toHaveBeenCalledWith(
          'You do not have permission to perform this action.'
        );
      }
    });
    
    // The following request should have been made
    const req = httpTestingController.expectOne('/api/test');
    
    // Respond with a 403 error
    req.flush('Forbidden', { status: 403, statusText: 'Forbidden' });
  });
});

