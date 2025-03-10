import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { ProductsApiService } from '../../../core/api/services/products-api.service';
import { Product } from '../../../core/api/models/domain.model';
import { ProductResponses } from '../../../core/api/models/responses.model';
import { NotificationService } from '../../../core/services/notification.service';
import { CommonRequests } from '../../../core/api/models/requests.model';

/**
 * Product pagination request parameters
 */
export interface ProductPaginationParams {
  page: number;
  limit: number;
}

/**
 * Product list response with metadata
 */
export interface ProductListResponse {
  products: Product[];
  total: number;
  page: number;
  limit: number;
}

/**
 * ProductService
 * 
 * Service for managing products in the application.
 * Provides methods for retrieving products and managing product state.
 */
@Injectable({
  providedIn: 'root'
})
export class ProductService {
  // Caching products
  private productsCache: Map<string, ProductListResponse> = new Map();
  
  constructor(
    private productsApiService: ProductsApiService,
    private notificationService: NotificationService
  ) { }
  
  /**
   * Get all products with pagination
   */
  getProducts(pagination: ProductPaginationParams): Observable<ProductListResponse> {
    const cacheKey = `all-${pagination.page}-${pagination.limit}`;
    
    // Check cache first (disable for now to ensure consistency)
    // if (this.productsCache.has(cacheKey)) {
    //   return of(this.productsCache.get(cacheKey)!);
    // }
    
    // Convert to API pagination format
    const apiPagination: CommonRequests.PaginationParams = {
      page: pagination.page,
      pageSize: pagination.limit
    };
    
    // Request from API
    return this.productsApiService.getProducts(apiPagination)
      .pipe(
        map(response => this.mapToProductListResponse(response, pagination)),
        tap(response => {
          // Update cache
          this.productsCache.set(cacheKey, response);
        }),
        catchError(error => {
          console.error('Error loading products:', error);
          this.notificationService.error('Failed to load products. Please try again.');
          throw error;
        })
      );
  }
  
  /**
   * Get a product by ID
   */
  getProductById(productId: string): Observable<Product> {
    return this.productsApiService.getProductById(productId)
      .pipe(
        map(response => this.mapToDomainProduct(response)),
        catchError(error => {
          this.notificationService.error('Failed to load product details. Please try again.');
          throw error;
        })
      );
  }
  
  /**
   * Clear all caches
   */
  clearCaches(): void {
    this.productsCache.clear();
  }
  
  /**
   * Map API response to ProductListResponse
   */
  private mapToProductListResponse(response: ProductResponses.ProductList, pagination: ProductPaginationParams): ProductListResponse {
    return {
      products: response.data.map(p => this.mapToDomainProduct(p)),
      total: response.totalCount,
      page: response.currentPage ? response.currentPage - 1 : pagination.page,
      limit: pagination.limit
    };
  }
  
  /**
   * Map API product to domain Product model
   */
  private mapToDomainProduct(product: ProductResponses.ProductDetails): Product {
    // Acessar propriedades de forma segura, respeitando a tipagem
    // Criar um objeto auxiliar que define um tipo de resposta estendido
    interface ExtendedProductDetails extends ProductResponses.ProductDetails {
      branchExternalId?: string;
      branchId?: string;
      branchName?: string;
    }
    
    // Converter para o tipo estendido - isso é seguro pois todos os campos
    // obrigatórios de ProductDetails já existem no objeto
    const extendedProduct = product as ExtendedProductDetails;
    
    return {
      id: product.id,
      name: product.name,
      price: product.price,
      description: product.description,
      category: product.category,
      sku: product.sku,
      stockQuantity: product.stockQuantity,
      status: product.status,
      createdAt: product.createdAt,
      updatedAt: product.updatedAt,
      // Usar valores da API quando disponíveis, caso contrário usar valores padrão
      branchExternalId: extendedProduct.branchExternalId || extendedProduct.branchId || 'default-branch-id',
      branchName: extendedProduct.branchName || 'Default Branch'
    };
  }
} 