import { TestBed, fakeAsync, tick } from '@angular/core/testing';
import { HttpTestingController } from '@angular/common/http/testing';
import { provideHttpClient, HttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ApiService } from './api.service';
import { LoggingService } from './logging.service';
import { environment } from '../../../environments/environment';

/**
 * Test suite for ApiService
 * 
 * @description
 * This suite tests the core API service that handles HTTP requests.
 * It verifies that the service correctly:
 * - Makes REST HTTP requests (GET, POST, PUT, DELETE)
 * - Handles query parameters and URL construction
 * - Sends proper request headers and bodies
 * - Processes response data correctly
 */
describe('ApiService', () => {
  let service: ApiService;
  let httpMock: HttpTestingController;
  let loggingServiceSpy: jasmine.SpyObj<LoggingService>;
  const apiUrl = environment.apiUrl;

  beforeEach(() => {
    // Create logging service spy
    loggingServiceSpy = jasmine.createSpyObj('LoggingService', ['logInfo']);
    
    TestBed.configureTestingModule({
      providers: [
        ApiService,
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: LoggingService, useValue: loggingServiceSpy }
      ]
    });
    
    service = TestBed.inject(ApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    // Verify no pending requests after each test
    httpMock.verify();
  });

  /**
   * Test service creation
   */
  it('should be created', () => {
    expect(service).toBeTruthy();
    expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith(
      'API Service initialized', 
      jasmine.objectContaining({ apiUrl })
    );
  });

  /**
   * Test GET method functionality
   */
  describe('get method', () => {
    /**
     * Test basic GET request with no parameters
     */
    it('should make a GET request with the correct URL', fakeAsync(() => {
      // Arrange
      const testData = { id: 1, name: 'Test' };
      const endpoint = 'products';
      
      // Act
      let result: any;
      service.get(endpoint).subscribe(data => {
        result = data;
      });
      
      // Assert
      const req = httpMock.expectOne(`${apiUrl}/${endpoint}`);
      expect(req.request.method).withContext('HTTP method').toBe('GET');
      req.flush(testData);
      tick();
      
      expect(result).withContext('Response data').toEqual(testData);
      expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith(
        'Making GET request', 
        jasmine.objectContaining({ url: `${apiUrl}/${endpoint}` })
      );
    }));
    
    /**
     * Test GET request with query parameters
     */
    it('should add query parameters to GET request when provided', fakeAsync(() => {
      // Arrange
      const testData = [{ id: 1, name: 'Test' }];
      const endpoint = 'products';
      const params = { category: 'electronics', sort: 'price' };
      
      // Act
      let result: any;
      service.get(endpoint, params).subscribe(data => {
        result = data;
      });
      
      // Assert
      const req = httpMock.expectOne(
        req => req.url === `${apiUrl}/${endpoint}` && 
               req.params.get('category') === 'electronics' &&
               req.params.get('sort') === 'price'
      );
      expect(req.request.method).withContext('HTTP method').toBe('GET');
      req.flush(testData);
      tick();
      
      expect(result).withContext('Response data').toEqual(testData);
    }));
    
    /**
     * Test parameter handling for null and undefined values
     */
    it('should skip null and undefined parameters but include zero values', fakeAsync(() => {
      // Arrange
      const testData = [{ id: 1, name: 'Test' }];
      const endpoint = 'products';
      const params = { 
        category: 'electronics', 
        sort: null, 
        filter: undefined, 
        count: 0,
        active: false
      };
      
      // Act
      let result: any;
      service.get(endpoint, params).subscribe(data => {
        result = data;
      });
      
      // Assert
      const req = httpMock.expectOne(
        req => req.url === `${apiUrl}/${endpoint}` && 
               req.params.get('category') === 'electronics' &&
               req.params.get('count') === '0' &&
               req.params.get('active') === 'false' &&
               !req.params.has('sort') &&
               !req.params.has('filter')
      );
      expect(req.request.method).withContext('HTTP method').toBe('GET');
      req.flush(testData);
      tick();
      
      expect(result).withContext('Response data').toEqual(testData);
    }));
  });

  /**
   * Test POST method functionality
   */
  describe('post method', () => {
    it('should make a POST request with the correct URL and body', fakeAsync(() => {
      // Arrange
      const testData = { name: 'New Product', price: 99.99 };
      const response = { id: 1, ...testData };
      const endpoint = 'products';
      
      // Act
      let result: any;
      service.post(endpoint, testData).subscribe(data => {
        result = data;
      });
      
      // Assert
      const req = httpMock.expectOne(`${apiUrl}/${endpoint}`);
      expect(req.request.method).withContext('HTTP method').toBe('POST');
      expect(req.request.body).withContext('Request body').toEqual(testData);
      req.flush(response);
      tick();
      
      expect(result).withContext('Response data').toEqual(response);
      expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith(
        'Making POST request', 
        jasmine.objectContaining({ 
          url: `${apiUrl}/${endpoint}`,
          data: testData 
        })
      );
    }));
  });

  /**
   * Test PUT method functionality
   */
  describe('put method', () => {
    it('should make a PUT request with the correct URL and body', fakeAsync(() => {
      // Arrange
      const id = 1;
      const testData = { name: 'Updated Product', price: 129.99 };
      const response = { id, ...testData };
      const endpoint = `products/${id}`;
      
      // Act
      let result: any;
      service.put(endpoint, testData).subscribe(data => {
        result = data;
      });
      
      // Assert
      const req = httpMock.expectOne(`${apiUrl}/${endpoint}`);
      expect(req.request.method).withContext('HTTP method').toBe('PUT');
      expect(req.request.body).withContext('Request body').toEqual(testData);
      req.flush(response);
      tick();
      
      expect(result).withContext('Response data').toEqual(response);
      expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith(
        'Making PUT request', 
        jasmine.objectContaining({ 
          url: `${apiUrl}/${endpoint}`,
          data: testData 
        })
      );
    }));
  });

  /**
   * Test DELETE method functionality
   */
  describe('delete method', () => {
    it('should make a DELETE request with the correct URL', fakeAsync(() => {
      // Arrange
      const id = 1;
      const endpoint = `products/${id}`;
      const response = { success: true };
      
      // Act
      let result: any;
      service.delete(endpoint).subscribe(data => {
        result = data;
      });
      
      // Assert
      const req = httpMock.expectOne(`${apiUrl}/${endpoint}`);
      expect(req.request.method).withContext('HTTP method').toBe('DELETE');
      req.flush(response);
      tick();
      
      expect(result).withContext('Response data').toEqual(response);
      expect(loggingServiceSpy.logInfo).toHaveBeenCalledWith(
        'Making DELETE request', 
        jasmine.objectContaining({ url: `${apiUrl}/${endpoint}` })
      );
    }));
  });
});
