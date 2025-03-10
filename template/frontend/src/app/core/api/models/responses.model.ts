/**
 * API Response Models
 * 
 * This file contains interfaces for all API response payloads.
 * Group related responses together and document them properly.
 */

/**
 * Common Response Models
 */
export namespace CommonResponses {
  /**
   * Base API Response that all responses should follow
   */
  export interface ApiResponse<T> {
    success: boolean;
    data?: T;
    error?: ErrorDetails;
    statusCode?: number;
    message?: string;
    meta?: MetaData;
  }

  /**
   * Error details for failed requests
   */
  export interface ErrorDetails {
    code?: string;
    message: string;
    details?: string;
    validationErrors?: { [key: string]: string[] };
  }

  /**
   * Metadata for responses, usually for pagination
   */
  export interface MetaData {
    totalCount?: number;
    page?: number;
    pageSize?: number;
    totalPages?: number;
  }

  /**
   * Simple success response
   */
  export interface SuccessResponse {
    success: boolean;
    message: string;
  }
}

/**
 * Authentication Response Models
 */
export namespace AuthResponses {
  /**
   * User Profile response from API
   */
  export interface UserProfile {
    id: string;
    name: string;
    email: string;
    phone: string;
    role: 'None' | 'Customer' | 'Manager' | 'Admin';
    status: number;
  }

  /**
   * Authentication Response with Token
   */
  export interface AuthResult {
    token: string;
    email: string;
    name: string;
    role: string;
  }
}

/**
 * User Response Models
 */
export namespace UserResponses {
  /**
   * User Details
   */
  export interface UserDetails {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    role: string;
    status: string;
    createdAt: string;
    updatedAt: string;
    lastLoginAt?: string;
    username?: string;
    phone?: string;
    address?: {
      city: string;
      street: string;
      number: number;
      zipcode: string;
      geolocation?: {
        lat: string;
        long: string;
      }
    };
  }

  /**
   * List of Users with pagination
   */
  export interface UserList {
    users: UserDetails[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
  }
}

/**
 * Product Response Models
 */
export namespace ProductResponses {
  /**
   * Product Details
   */
  export interface ProductDetails {
    id: string;
    name: string;
    price: number;
    description: string;
    category: string;
    sku: string;
    stockQuantity: number;
    status: string;
    createdAt: string;
    updatedAt: string | null;
  }

  /**
   * List of Products with pagination
   */
  export interface ProductList {
    currentPage: number;
    totalPages: number;
    totalCount: number;
    data: ProductDetails[];
    success: boolean;
    message: string;
    errors: string[];
  }

  /**
   * List of Product Categories
   */
  export interface CategoryList {
    categories: string[];
  }
}

/**
 * Cart Response Models
 */
export namespace CartResponses {
  /**
   * Cart Item in Response
   */
  export interface CartItemDetails {
    productId: string;
    quantity: number;
    product?: ProductResponses.ProductDetails;
  }

  /**
   * Cart Details
   */
  export interface CartDetails {
    id: string;
    userId: string;
    date: string;
    products: CartItemDetails[];
    isCompleted?: boolean;
    totalPrice?: number;
  }

  /**
   * List of Carts with pagination
   */
  export interface CartList {
    data: CartDetails[];
    totalItems: number;
    currentPage: number;
    totalPages: number;
  }

  /**
   * Order History Entry
   */
  export interface OrderHistoryItem {
    id: string;
    cartId: string;
    orderDate: string;
    totalPrice: number;
    status: string;
    products: CartItemDetails[];
  }

  /**
   * Order History List
   */
  export interface OrderHistory {
    orders: OrderHistoryItem[];
    totalCount: number;
  }
} 