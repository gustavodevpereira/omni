/**
 * API Request Models
 * 
 * This file contains interfaces for all API request payloads.
 * Group related requests together and document them properly.
 */

/**
 * Authentication Request Models
 */
export namespace AuthRequests {
  /**
   * Login Request
   */
  export interface Login {
    email: string;
    password: string;
    rememberMe?: boolean;
  }

  /**
   * Registration Request
   */
  export interface Register {
    email: string;
    password: string;
    name: string;
    confirmPassword: string;
    phone?: string;
  }

  /**
   * Forgot Password Request
   */
  export interface ForgotPassword {
    email: string;
  }

  /**
   * Reset Password Request
   */
  export interface ResetPassword {
    token: string;
    password: string;
    confirmPassword: string;
  }
}

/**
 * User Request Models
 */
export namespace UserRequests {
  /**
   * Create User Request
   */
  export interface Create {
    email: string;
    firstName: string;
    lastName: string;
    role: string;
    password?: string;
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
   * Update User Request
   */
  export interface Update {
    firstName?: string;
    lastName?: string;
    email?: string;
    role?: string;
    username?: string;
    phone?: string;
    address?: {
      city?: string;
      street?: string;
      number?: number;
      zipcode?: string;
      geolocation?: {
        lat?: string;
        long?: string;
      }
    };
  }

  /**
   * Update Password Request
   */
  export interface UpdatePassword {
    currentPassword: string;
    newPassword: string;
    confirmPassword: string;
  }

  /**
   * User Search Filters
   */
  export interface SearchFilters {
    query?: string;
    role?: string;
    status?: string;
    sortBy?: string;
    sortDirection?: 'asc' | 'desc';
  }
}

/**
 * Common Request Models
 */
export namespace CommonRequests {
  /**
   * Pagination Parameters
   */
  export interface PaginationParams {
    page: number;
    pageSize: number;
    sortBy?: string;
    sortDirection?: 'asc' | 'desc';
  }
}

/**
 * Product Request Models
 */
export namespace ProductRequests {
  /**
   * Product Search Filters
   */
  export interface SearchFilters {
    title?: string;
    name?: string;
    category?: string;
    minPrice?: number;
    maxPrice?: number;
    sortBy?: string;
    sortDirection?: 'asc' | 'desc';
  }

  /**
   * Create Product Request
   */
  export interface Create {
    name: string;
    price: number;
    description: string;
    category: string;
    sku: string;
    stockQuantity: number;
    status: string;
  }

  /**
   * Update Product Request
   */
  export interface Update {
    name?: string;
    price?: number;
    description?: string;
    category?: string;
    sku?: string;
    stockQuantity?: number;
    status?: string;
  }
}

/**
 * Cart Request Models
 */
export namespace CartRequests {
  /**
   * Cart Item in Request
   */
  export interface CartItem {
    productId: string;
    quantity: number;
  }

  /**
   * Create Cart Request
   */
  export interface Create {
    userId: string;
    products: CartItem[];
  }

  /**
   * Update Cart Request
   */
  export interface Update {
    userId?: string;
    products?: CartItem[];
  }

  /**
   * Add To Cart Request
   */
  export interface AddItem {
    productId: string;
    quantity: number;
  }

  /**
   * Update Cart Item Request
   */
  export interface UpdateItem {
    quantity: number;
  }

  /**
   * Complete Cart (Checkout) Request
   */
  export interface CompleteCart {
    cartId: string;
  }
} 