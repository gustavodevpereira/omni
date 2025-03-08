import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { HttpTestingController } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { provideRouter } from '@angular/router';
import { take } from 'rxjs/operators';

import { AuthService } from './auth.service';
import { LoggingService } from './logging.service';
import { NotificationService } from './notification.service';
import { environment } from '../../../environments/environment';
import { User, UserRole, AuthRequest, AuthResponse } from '../../shared/models/user.model';
import { ApiResponseWithData } from '../../shared/models/api-response.models';

/**
 * Test suite for AuthService
 * 
 * @description
 * Tests the authentication service that handles user login/logout, session management,
 * and user operations. This tests the proper behavior of token handling, user object
 * management, and API interactions.
 */
describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let routerSpy: jasmine.SpyObj<Router>;
  let loggingServiceSpy: jasmine.SpyObj<LoggingService>;
  let notificationServiceSpy: jasmine.SpyObj<NotificationService>;
  const apiUrl = environment.apiUrl;

  // Storage mock to simulate localStorage behavior
  let storedItems: {[key: string]: string} = {};

  beforeEach(() => {
    // Reset storage mock
    storedItems = {};
    
    // Setup spies
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    loggingServiceSpy = jasmine.createSpyObj('LoggingService', ['logInfo', 'logError', 'logWarning']);
    notificationServiceSpy = jasmine.createSpyObj('NotificationService', ['success', 'error', 'info', 'warning']);
    
    // Setup localStorage spy implementation with a mock storage
    spyOn(localStorage, 'setItem').and.callFake((key, value) => {
      storedItems[key] = value;
    });
    
    spyOn(localStorage, 'getItem').and.callFake((key) => {
      return storedItems[key] || null;
    });
    
    spyOn(localStorage, 'removeItem').and.callFake((key) => {
      delete storedItems[key];
    });
    
    TestBed.configureTestingModule({
      providers: [
        AuthService,
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([]),
        { provide: Router, useValue: routerSpy },
        { provide: LoggingService, useValue: loggingServiceSpy },
        { provide: NotificationService, useValue: notificationServiceSpy }
      ]
    });
    
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    // Verify no pending requests
    httpMock.verify();
  });

  /**
   * Test service creation
   */
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  /**
   * Test login functionality
   */
  describe('login', () => {
    it('should authenticate user and store token and user data', () => {
      // Arrange
      const credentials: AuthRequest = {
        email: 'test@example.com',
        password: 'password123'
      };
      
      const authResponse: AuthResponse = {
        token: 'test-token',
        email: 'test@example.com',
        name: 'Test User',
        role: 'customer'
      };
      
      const apiResponse: ApiResponseWithData<AuthResponse> = {
        success: true,
        message: 'Authentication successful',
        data: authResponse,
        errors: []
      };
      
      // Act
      service.login(credentials).pipe(take(1)).subscribe(response => {
        // Assert
        expect(response).toEqual(apiResponse);
        expect(localStorage.setItem).toHaveBeenCalledWith('auth_token', authResponse.token);
      });
      
      // Expect request and mock response
      const req = httpMock.expectOne(`${apiUrl}/auth`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(credentials);
      req.flush(apiResponse);
    });
  });

  /**
   * Test logout functionality
   */
  describe('logout', () => {
    it('should clear token, user data and navigate to login page', () => {
      // Act
      service.logout();
      
      // Assert
      expect(localStorage.removeItem).toHaveBeenCalledWith('auth_token');
      expect(localStorage.removeItem).toHaveBeenCalledWith('user_data');
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/auth/login']);
      expect(notificationServiceSpy.info).toHaveBeenCalled();
    });
  });

  /**
   * Test isLoggedIn functionality
   */
  describe('isLoggedIn', () => {
    it('should return true when token exists', () => {
      // Arrange
      storedItems['auth_token'] = 'test-token';
      
      // Act & Assert
      expect(service.isLoggedIn()).toBeTrue();
    });

    it('should return false when token does not exist', () => {
      // Arrange - token is not set
      
      // Act & Assert
      expect(service.isLoggedIn()).toBeFalse();
    });
  });

  /**
   * Test register functionality
   */
  describe('register', () => {
    it('should register a new user', () => {
      // Arrange
      const registerData = {
        username: 'testuser',
        email: 'test@example.com',
        password: 'password123'
      };
      
      const userDTO = {
        id: '1',
        email: registerData.email,
        name: registerData.username,
        role: UserRole.CUSTOMER
      };
      
      const apiResponse: ApiResponseWithData<typeof userDTO> = {
        success: true,
        message: 'User registered successfully',
        data: userDTO,
        errors: []
      };
      
      // Act
      service.register(registerData).pipe(take(1)).subscribe(response => {
        // Assert
        expect(response).toEqual(apiResponse);
        expect(notificationServiceSpy.success).toHaveBeenCalled();
      });
      
      // Expect request and mock response
      const req = httpMock.expectOne(`${apiUrl}/users`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(registerData);
      req.flush(apiResponse);
    });
  });

  /**
   * Test token management
   */
  describe('token management', () => {
    it('should retrieve token from localStorage', () => {
      // Arrange
      const token = 'test-token';
      storedItems['auth_token'] = token;
      
      // Act
      const result = service.getToken();
      
      // Assert
      expect(result).toBe(token);
      expect(localStorage.getItem).toHaveBeenCalledWith('auth_token');
    });
  });

  /**
   * Test user management
   */
  describe('user management', () => {
    it('should get user profile', () => {
      // Arrange
      const userId = '1';
      const userDTO = {
        id: userId,
        email: 'test@example.com',
        name: 'Test User',
        role: 'customer'
      };
      
      const apiResponse: ApiResponseWithData<typeof userDTO> = {
        success: true,
        message: 'User found',
        data: userDTO,
        errors: []
      };
      
      // Act
      service.getUserById(userId).pipe(take(1)).subscribe(response => {
        // Assert
        expect(response).toEqual(apiResponse);
      });
      
      // Expect request and mock response
      const req = httpMock.expectOne(`${apiUrl}/users/${userId}`);
      expect(req.request.method).toBe('GET');
      req.flush(apiResponse);
    });

    it('should update user profile', () => {
      // Arrange
      const userId = '1';
      const updateData = { name: 'Updated Name' };
      
      const userDTO = {
        id: userId,
        email: 'test@example.com',
        name: updateData.name,
        role: 'customer'
      };
      
      const apiResponse: ApiResponseWithData<typeof userDTO> = {
        success: true,
        message: 'User updated',
        data: userDTO,
        errors: []
      };
      
      // Act
      service.updateUser(userId, updateData).pipe(take(1)).subscribe(response => {
        // Assert
        expect(response).toEqual(apiResponse);
        expect(notificationServiceSpy.success).toHaveBeenCalled();
      });
      
      // Expect request and mock response
      const req = httpMock.expectOne(`${apiUrl}/users/${userId}`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(updateData);
      req.flush(apiResponse);
    });

    it('should delete user account', () => {
      // Arrange
      const userId = '1';
      
      const apiResponse: ApiResponseWithData<null> = {
        success: true,
        message: 'User deleted',
        data: null,
        errors: []
      };
      
      // Act
      service.deleteUser(userId).pipe(take(1)).subscribe(response => {
        // Assert
        expect(response).toEqual(apiResponse);
        expect(notificationServiceSpy.success).toHaveBeenCalled();
      });
      
      // Expect request and mock response
      const req = httpMock.expectOne(`${apiUrl}/users/${userId}`);
      expect(req.request.method).toBe('DELETE');
      req.flush(apiResponse);
    });
  });
});


