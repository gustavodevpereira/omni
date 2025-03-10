import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

import { ApiService, RequestOptions } from './api.service';
import { environment } from '../../../environments/environment';

/**
 * Unit tests for ApiService
 */
describe('ApiService', () => {
  let service: ApiService;
  let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;
  const apiBaseUrl = environment.apiUrl;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ApiService]
    });

    // Inject services
    service = TestBed.inject(ApiService);
    httpClient = TestBed.inject(HttpClient);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  // Verify no outstanding requests after each test
  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  /**
   * Test GET request
   */
  it('should make GET request with correct URL and default options', () => {
    const testData = { id: 1, name: 'Test Data' };
    const endpoint = '/test-endpoint';

    // Execute service call with expectation
    service.get<any>(endpoint).subscribe(data => {
      expect(data).toEqual(testData);
    });

    // Check request
    const req = httpTestingController.expectOne(request => {
      return request.method === 'GET' && 
             request.url.includes('test-endpoint');
    });

    // Return mock response
    req.flush(testData);
  });

  /**
   * Test GET request with options
   */
  it('should make GET request with custom options', () => {
    const testData = { id: 1, name: 'Test Data' };
    const endpoint = '/test-endpoint';
    const options: RequestOptions = {
      headers: { 'Custom-Header': 'TestValue' },
      params: { param1: 'value1', param2: 123 }
    };

    // Execute service call with expectation
    service.get<any>(endpoint, options).subscribe(data => {
      expect(data).toEqual(testData);
    });

    // Check request
    const req = httpTestingController.expectOne(request => {
      return request.method === 'GET' && 
             request.url.includes('test-endpoint') &&
             request.headers.has('Custom-Header') &&
             request.params.has('param1') &&
             request.params.has('param2');
    });

    // Return mock response
    req.flush(testData);
  });

  /**
   * Test POST request
   */
  it('should make POST request with correct data and URL', () => {
    const testData = { id: 1, name: 'Test Data' };
    const endpoint = '/test-endpoint';
    const requestBody = { name: 'New Item' };

    // Execute service call with expectation
    service.post<any>(endpoint, requestBody).subscribe(data => {
      expect(data).toEqual(testData);
    });

    // Check request
    const req = httpTestingController.expectOne(request => {
      return request.method === 'POST' && 
             request.url.includes('test-endpoint');
    });
    expect(req.request.body).toEqual(requestBody);

    // Return mock response
    req.flush(testData);
  });

  /**
   * Test PUT request
   */
  it('should make PUT request with correct data and URL', () => {
    const testData = { id: 1, name: 'Updated Data' };
    const endpoint = '/test-endpoint/1';
    const requestBody = { name: 'Updated Item' };

    // Execute service call with expectation
    service.put<any>(endpoint, requestBody).subscribe(data => {
      expect(data).toEqual(testData);
    });

    // Check request
    const req = httpTestingController.expectOne(request => {
      return request.method === 'PUT' && 
             request.url.includes('test-endpoint/1');
    });
    expect(req.request.body).toEqual(requestBody);

    // Return mock response
    req.flush(testData);
  });

  /**
   * Test PATCH request
   */
  it('should make PATCH request with correct data and URL', () => {
    const testData = { id: 1, name: 'Updated Data' };
    const endpoint = '/test-endpoint/1';
    const requestBody = { name: 'Updated Item' };

    // Execute service call with expectation
    service.patch<any>(endpoint, requestBody).subscribe(data => {
      expect(data).toEqual(testData);
    });

    // Check request
    const req = httpTestingController.expectOne(request => {
      return request.method === 'PATCH' && 
             request.url.includes('test-endpoint/1');
    });
    expect(req.request.body).toEqual(requestBody);

    // Return mock response
    req.flush(testData);
  });

  /**
   * Test DELETE request
   */
  it('should make DELETE request with correct URL', () => {
    const testData = { success: true };
    const endpoint = '/test-endpoint/1';

    // Execute service call with expectation
    service.delete<any>(endpoint).subscribe(data => {
      expect(data).toEqual(testData);
    });

    // Check request
    const req = httpTestingController.expectOne(request => {
      return request.method === 'DELETE' && 
             request.url.includes('test-endpoint/1');
    });

    // Return mock response
    req.flush(testData);
  });

  /**
   * Test error handling - HTTP error
   */
  it('should handle HTTP errors', (done) => {
    const endpoint = '/test-endpoint';
    const errorMessage = 'Simulated network error';
    const errorStatus = 404;

    // Execute service call with expectation
    service.get<any>(endpoint).subscribe(
      () => {
        fail('Expected an error, not successful response');
      },
      error => {
        expect(error.status).toBe(errorStatus);
        expect(error.statusText).toBe(errorMessage);
        done();
      }
    );

    // Create HTTP error response
    const req = httpTestingController.expectOne(request => {
      return request.method === 'GET' && 
             request.url.includes('test-endpoint');
    });
    req.flush(errorMessage, { status: errorStatus, statusText: errorMessage });
  });

  /**
   * Test parameter serialization
   */
  it('should handle parameter serialization correctly', () => {
    const testData = { result: 'success' };
    const endpoint = '/parameters-test';
    const options: RequestOptions = {
      params: { 
        stringParam: 'test',
        numberParam: 123,
        booleanParam: true
      }
    };

    // Execute service call with expectation
    service.get<any>(endpoint, options).subscribe(data => {
      expect(data).toEqual(testData);
    });

    // Check request
    const req = httpTestingController.expectOne(request => {
      return request.method === 'GET' && 
             request.url.includes('parameters-test') &&
             request.params.has('stringParam') &&
             request.params.has('numberParam') &&
             request.params.has('booleanParam');
    });

    // Return mock response
    req.flush(testData);
  });

  /**
   * Test endpoint formatting
   */
  it('should format endpoints correctly regardless of slashes', () => {
    const testData = { result: 'success' };
    
    // Test with leading slash
    service.get<any>('/with-slash').subscribe(data => {
      expect(data).toEqual(testData);
    });
    
    const req1 = httpTestingController.expectOne(request => {
      return request.method === 'GET' && 
             request.url.includes('with-slash');
    });
    req1.flush(testData);
    
    // Test without leading slash
    service.get<any>('without-slash').subscribe(data => {
      expect(data).toEqual(testData);
    });
    
    const req2 = httpTestingController.expectOne(request => {
      return request.method === 'GET' && 
             request.url.includes('without-slash');
    });
    req2.flush(testData);
  });
}); 