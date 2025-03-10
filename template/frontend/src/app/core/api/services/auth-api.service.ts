import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiBaseService } from './api-base.service';
import { API_ENDPOINTS } from '../config/api.config';
import { AuthRequests } from '../models/requests.model';
import { AuthResponses, CommonResponses } from '../models/responses.model';

/**
 * Authentication API Service
 * 
 * Handles all API calls related to authentication and user management.
 * Extends the ApiBaseService to utilize common HTTP functionality.
 */
@Injectable({
  providedIn: 'root'
})
export class AuthApiService {

  constructor(private apiService: ApiBaseService) { }

  /**
   * Login user with email and password
   * 
   * @param loginData - Login credentials (email, password)
   * @returns Observable with authentication result (token and user)
   */
  login(loginData: AuthRequests.Login): Observable<AuthResponses.AuthResult> {
    return this.apiService
      .post<CommonResponses.ApiResponse<AuthResponses.AuthResult>>(
        API_ENDPOINTS.AUTH.LOGIN, 
        loginData
      )
      .pipe(
        map(response => {
          if (response.success && response.data) {
            return response.data;
          }
          throw new Error(response.error?.message || 'Login failed');
        })
      );
  }

  /**
   * Register a new user
   * 
   * @param registerData - Registration data
   * @returns Observable with authentication result (token and user)
   */
  register(registerData: AuthRequests.Register): Observable<AuthResponses.AuthResult> {
    return this.apiService
      .post<CommonResponses.ApiResponse<AuthResponses.AuthResult>>(
        API_ENDPOINTS.AUTH.REGISTER, 
        registerData
      )
      .pipe(
        map(response => {
          if (response.success && response.data) {
            return response.data;
          }
          throw new Error(response.error?.message || 'Registration failed');
        })
      );
  }

  /**
   * Request password reset for a user
   * 
   * @param data - Forgot password data containing email
   * @returns Observable with success message
   */
  forgotPassword(data: AuthRequests.ForgotPassword): Observable<string> {
    return this.apiService
      .post<CommonResponses.ApiResponse<null>>(
        API_ENDPOINTS.AUTH.FORGOT_PASSWORD, 
        data
      )
      .pipe(
        map(response => {
          if (response.success) {
            return response.message || 'Password reset instructions sent to your email';
          }
          throw new Error(response.error?.message || 'Failed to process request');
        })
      );
  }

  /**
   * Reset user password using token
   * 
   * @param data - Reset password data with token and new password
   * @returns Observable with success message
   */
  resetPassword(data: AuthRequests.ResetPassword): Observable<string> {
    return this.apiService
      .post<CommonResponses.ApiResponse<null>>(
        API_ENDPOINTS.AUTH.RESET_PASSWORD, 
        data
      )
      .pipe(
        map(response => {
          if (response.success) {
            return response.message || 'Password reset successful';
          }
          throw new Error(response.error?.message || 'Failed to reset password');
        })
      );
  }
} 