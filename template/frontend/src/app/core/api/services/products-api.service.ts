import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiBaseService } from './api-base.service';
import { API_ENDPOINTS } from '../config/api.config';
import { CommonRequests } from '../models/requests.model';
import { ProductResponses } from '../models/responses.model';

/**
 * Products API Service
 * 
 * Handles all API calls related to products.
 * Uses the ApiBaseService for HTTP communication.
 */
@Injectable({
  providedIn: 'root'
})
export class ProductsApiService {

  constructor(private apiService: ApiBaseService) { }

  /**
   * Get a paginated list of products
   * 
   * @param params - Pagination parameters
   * @returns Observable with paginated product list
   */
  getProducts(
    params: CommonRequests.PaginationParams
  ): Observable<ProductResponses.ProductList> {
    // Build query parameters for pagination
    const queryParams: any = {
      PageNumber: params.page + 1, // Convert to 1-indexed for API
      PageSize: params.pageSize
    };

    // Add sorting if provided
    if (params.sortBy) {
      const direction = params.sortDirection || 'asc';
      queryParams.sortBy = params.sortBy;
      queryParams.sortDirection = direction;
    }

    return this.apiService
      .get<ProductResponses.ProductList>(
        API_ENDPOINTS.PRODUCTS.BASE,
        { params: queryParams }
      );
  }

  /**
   * Get a product by ID
   * 
   * @param productId - Product ID
   * @returns Observable with product details
   */
  getProductById(productId: string): Observable<ProductResponses.ProductDetails> {
    return this.apiService
      .get<ProductResponses.ProductDetails>(
        API_ENDPOINTS.PRODUCTS.DETAILS(productId)
      );
  }

  /**
   * Get all product categories
   * 
   * @returns Observable with list of categories
   */
  getCategories(): Observable<string[]> {
    return this.apiService
      .get<string[]>(API_ENDPOINTS.PRODUCTS.CATEGORIES);
  }

  /**
   * Get products by category
   * 
   * @param category - Category name
   * @param params - Pagination parameters
   * @returns Observable with paginated product list
   */
  getProductsByCategory(
    category: string,
    params: CommonRequests.PaginationParams
  ): Observable<ProductResponses.ProductList> {
    const queryParams: any = {
      page: params.page,
      pageSize: params.pageSize,
      category: category
    };

    // Add sorting if provided
    if (params.sortBy) {
      const direction = params.sortDirection || 'asc';
      queryParams.sortBy = params.sortBy;
      queryParams.sortDirection = direction;
    }

    return this.apiService
      .get<ProductResponses.ProductList>(
        API_ENDPOINTS.PRODUCTS.BASE,
        { params: queryParams }
      );
  }
} 