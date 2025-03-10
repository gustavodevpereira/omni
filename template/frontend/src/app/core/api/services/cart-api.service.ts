import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { API_ENDPOINTS } from '../config/api.config';

/**
 * Backend Cart API Integration
 * 
 * @description Namespace containing interface definitions for the backend cart API
 */
export namespace BackendCart {
  /**
   * Product information for cart creation request
   */
  export interface Product {
    /** ID of the product */
    productId: string;
    
    /** Name of the product */
    productName: string;
    
    /** Quantity of the product to be ordered */
    quantity: number;
    
    /** Unit price of the product */
    unitPrice: number;
  }

  /**
   * Cart creation request payload
   */
  export interface CreateCartRequest {
    /** ID of the customer placing the order */
    customerId: string;
    
    /** Name of the customer placing the order */
    customerName: string;
    
    /** ID of the branch where the order is being placed */
    branchId: string;
    
    /** Name of the branch where the order is being placed */
    branchName: string;
    
    /** List of products in the cart */
    products: Product[];
  }

  /**
   * Product information in cart response
   */
  export interface ProductResponse {
    /** ID of the product */
    productId: string;
    
    /** Name of the product */
    name: string;
    
    /** Unit price of the product */
    price: number;
    
    /** Quantity ordered */
    quantity: number;
    
    /** Subtotal before discount (price * quantity) */
    subtotal: number;
    
    /** Discount percentage applied to the product (0-1) */
    discountPercentage: number;
    
    /** Total discount amount */
    discountAmount: number;
    
    /** Final amount after discount */
    finalAmount: number;
  }

  /**
   * Cart/Order information in response
   */
  export interface CartResponse {
    /** Unique identifier for the cart/order */
    id: string;
    
    /** Customer ID */
    customerId: string;
    
    /** Customer name */
    customerName: string;
    
    /** Customer email */
    customerEmail?: string;
    
    /** External reference ID for the branch */
    branchExternalId: string;
    
    /** Branch name */
    branchName: string;
    
    /** Date of the order */
    date: string;
    
    /** Creation timestamp of the cart/order */
    createdOn: string;
    
    /** Current status of the cart/order */
    status: string;
    
    /** Total amount before discounts */
    totalAmount: number;
    
    /** Total discount amount */
    totalDiscount: number;
    
    /** Final amount after discounts */
    finalAmount: number;
    
    /** List of products in the cart/order */
    products: ProductResponse[];
  }

  /**
   * Generic API response wrapper
   */
  export interface ApiResponse<T> {
    /** Indicates if the operation was successful */
    success: boolean;
    
    /** Message describing the result of the operation */
    message: string;
    
    /** List of errors that occurred during the operation, if any */
    errors?: Array<{
      error: string;
      detail: string;
    }>;
    
    /** Response data */
    data: T;
  }

  /**
   * Paginated cart list response
   */
  export interface CartListResponse {
    /** Current page number */
    currentPage: number;
    
    /** Total number of pages available */
    totalPages: number;
    
    /** Total count of items across all pages */
    totalCount: number;
    
    /** List of carts/orders in the current page */
    data: CartResponse[];
    
    /** Indicates if the operation was successful */
    success: boolean;
    
    /** Message describing the result of the operation */
    message: string;
    
    /** List of errors that occurred during the operation, if any */
    errors: any[];
  }

  /**
   * Product information for discount calculation request
   */
  export interface DiscountProduct {
    /** ID of the product */
    productId: string;
    
    /** Name of the product */
    name: string;
    
    /** Price of the product */
    price: number;
    
    /** Quantity of the product to be ordered */
    quantity: number;
  }

  /**
   * Discount calculation request payload
   */
  export interface CalculateDiscountRequest {
    /** ID of the branch where the order is being placed */
    branchExternalId: string;
    
    /** Name of the branch where the order is being placed */
    branchName: string;
    
    /** Date of the order */
    date: string;
    
    /** List of products in the cart */
    products: DiscountProduct[];
  }
}

/**
 * Cart API Service
 * 
 * @description Service for interacting with the backend cart API
 * Provides methods for creating, retrieving, and managing shopping carts and orders
 */
@Injectable({
  providedIn: 'root'
})
export class CartApiService {
  /** Base URL for the backend API */
  private backendApiUrl: string;

  /**
   * Constructor
   * 
   * @param http - Angular HTTP client for making API requests
   */
  constructor(private http: HttpClient) { 
    // Ensure no duplicate slashes in the URL
    const apiUrl = environment.apiUrl.endsWith('/') 
      ? environment.apiUrl.slice(0, -1) 
      : environment.apiUrl;
    
    this.backendApiUrl = `${apiUrl}/Carts`;
  }
  
  /**
   * Create a new cart in the backend
   * 
   * @param cartData - Cart data to be sent to the backend
   * @returns Observable with the API response containing the created cart
   */
  createCartBackend(cartData: BackendCart.CreateCartRequest): Observable<BackendCart.ApiResponse<BackendCart.CartResponse>> {
    return this.http.post<BackendCart.ApiResponse<BackendCart.CartResponse>>(
      this.backendApiUrl, 
      cartData, 
      { headers: this.getAuthHeaders() }
    );
  }

  /**
   * Get a paginated list of carts from the backend
   * 
   * @param page - Page number (1-based)
   * @param pageSize - Number of items per page
   * @returns Observable with the paginated cart list response
   */
  getCartListBackend(page: number = 1, pageSize: number = 10): Observable<BackendCart.CartListResponse> {
    const url = `${this.backendApiUrl}?pageNumber=${page}&pageSize=${pageSize}`;
    return this.http.get<BackendCart.CartListResponse>(url, { headers: this.getAuthHeaders() });
  }

  /**
   * Get a specific cart by ID
   * 
   * @param cartId - ID of the cart to retrieve
   * @returns Observable with the API response containing the cart details
   */
  getCartByIdBackend(cartId: string): Observable<BackendCart.ApiResponse<BackendCart.CartResponse>> {
    return this.http.get<BackendCart.ApiResponse<BackendCart.CartResponse>>(
      `${this.backendApiUrl}/${cartId}`,
      { headers: this.getAuthHeaders() }
    );
  }

  /**
   * Update an existing cart
   * 
   * @param cartId - ID of the cart to update
   * @param cartData - Updated cart data
   * @returns Observable with the API response containing the updated cart
   */
  updateCartBackend(cartId: string, cartData: Partial<BackendCart.CreateCartRequest>): Observable<BackendCart.ApiResponse<BackendCart.CartResponse>> {
    return this.http.put<BackendCart.ApiResponse<BackendCart.CartResponse>>(
      `${this.backendApiUrl}/${cartId}`, 
      cartData,
      { headers: this.getAuthHeaders() }
    );
  }

  /**
   * Delete a cart
   * 
   * @param cartId - ID of the cart to delete
   * @returns Observable with the API response
   */
  deleteCartBackend(cartId: string): Observable<BackendCart.ApiResponse<any>> {
    return this.http.delete<BackendCart.ApiResponse<any>>(
      `${this.backendApiUrl}/${cartId}`,
      { headers: this.getAuthHeaders() }
    );
  }
  
  /**
   * Get authentication headers for API requests
   * 
   * @returns HttpHeaders with authentication token
   * @private
   */
  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('auth_token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  /**
   * Calculate discounts for products in the cart
   * 
   * @param request - The cart request with products to calculate discounts for
   * @returns Observable with API response containing the updated cart with discounts
   */
  calculateDiscounts(request: BackendCart.CalculateDiscountRequest): Observable<BackendCart.ApiResponse<BackendCart.CartResponse>> {
    return this.http.post<BackendCart.ApiResponse<BackendCart.CartResponse>>(
      API_ENDPOINTS.CARTS.CALCULATE_DISCOUNT,
      request,
      { headers: this.getAuthHeaders() }
    );
  }
} 