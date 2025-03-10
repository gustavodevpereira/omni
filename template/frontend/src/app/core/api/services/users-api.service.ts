import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiBaseService } from './api-base.service';
import { API_ENDPOINTS } from '../config/api.config';
import { UserRequests, CommonRequests } from '../models/requests.model';
import { UserResponses, CommonResponses } from '../models/responses.model';

/**
 * Users API Service
 * 
 * Handles all API calls related to user management.
 * Extends the ApiBaseService to utilize common HTTP functionality.
 */
@Injectable({
  providedIn: 'root'
})
export class UsersApiService {

  constructor(private apiService: ApiBaseService) { }

  /**
   * Get a list of users with pagination
   * 
   * @param params - Pagination parameters
   * @param filters - Optional filters
   * @returns Observable with paginated user list
   */
  getUsers(
    params: CommonRequests.PaginationParams,
    filters?: UserRequests.SearchFilters
  ): Observable<UserResponses.UserList> {
    return this.apiService
      .get<CommonResponses.ApiResponse<UserResponses.UserList>>(
        API_ENDPOINTS.USERS.BASE,
        { params: { ...params, ...filters } }
      )
      .pipe(
        map(response => {
          if (response.success && response.data) {
            return response.data;
          }
          throw new Error(response.error?.message || 'Failed to fetch users');
        })
      );
  }

  /**
   * Get a specific user by ID
   * 
   * @param userId - The user ID
   * @returns Observable with user details
   */
  getUserById(userId: string): Observable<UserResponses.UserDetails> {
    return this.apiService
      .get<CommonResponses.ApiResponse<UserResponses.UserDetails>>(
        API_ENDPOINTS.USERS.DETAILS(userId)
      )
      .pipe(
        map(response => {
          if (response.success && response.data) {
            return response.data;
          }
          throw new Error(response.error?.message || 'User not found');
        })
      );
  }

  /**
   * Create a new user
   * 
   * @param userData - User data
   * @returns Observable with created user details
   */
  createUser(userData: UserRequests.Create): Observable<UserResponses.UserDetails> {
    return this.apiService
      .post<CommonResponses.ApiResponse<UserResponses.UserDetails>>(
        API_ENDPOINTS.USERS.BASE,
        userData
      )
      .pipe(
        map(response => {
          if (response.success && response.data) {
            return response.data;
          }
          throw new Error(response.error?.message || 'Failed to create user');
        })
      );
  }

  /**
   * Update a user
   * 
   * @param userId - The user ID
   * @param userData - User data to update
   * @returns Observable with updated user details
   */
  updateUser(userId: string, userData: UserRequests.Update): Observable<UserResponses.UserDetails> {
    return this.apiService
      .put<CommonResponses.ApiResponse<UserResponses.UserDetails>>(
        API_ENDPOINTS.USERS.UPDATE(userId),
        userData
      )
      .pipe(
        map(response => {
          if (response.success && response.data) {
            return response.data;
          }
          throw new Error(response.error?.message || 'Failed to update user');
        })
      );
  }

  /**
   * Update user password
   * 
   * @param userId - The user ID
   * @param passwordData - Password data
   * @returns Observable with success message
   */
  updatePassword(userId: string, passwordData: UserRequests.UpdatePassword): Observable<string> {
    return this.apiService
      .put<CommonResponses.ApiResponse<null>>(
        `${API_ENDPOINTS.USERS.UPDATE(userId)}/password`,
        passwordData
      )
      .pipe(
        map(response => {
          if (response.success) {
            return response.message || 'Password updated successfully';
          }
          throw new Error(response.error?.message || 'Failed to update password');
        })
      );
  }

  /**
   * Delete a user
   * 
   * @param userId - The user ID
   * @returns Observable with success status
   */
  deleteUser(userId: string): Observable<boolean> {
    return this.apiService
      .delete<CommonResponses.ApiResponse<null>>(
        API_ENDPOINTS.USERS.DELETE(userId)
      )
      .pipe(
        map(response => {
          return response.success;
        })
      );
  }
} 