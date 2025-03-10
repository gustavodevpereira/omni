/**
 * API Configuration
 * 
 * Contains constants and configuration values for API communication.
 * This file centralizes all API-related configuration.
 */

import { environment } from '../../../../environments/environment';

/**
 * API endpoints configuration
 */
export const API_ENDPOINTS = {
  // Auth endpoints
  AUTH: {
    LOGIN: '/Auth',
    REGISTER: '/auth/register',
    LOGOUT: '/auth/logout',
    REFRESH_TOKEN: '/auth/refresh-token',
    PROFILE: '/auth/profile',
    FORGOT_PASSWORD: '/auth/forgot-password',
    RESET_PASSWORD: '/auth/reset-password',
  },
  
  // User endpoints
  USERS: {
    BASE: '/users',
    DETAILS: (id: string) => `/users/${id}`,
    UPDATE: (id: string) => `/users/${id}`,
    DELETE: (id: string) => `/users/${id}`,
  },
  
  // Product endpoints
  PRODUCTS: {
    BASE: '/products',
    DETAILS: (id: string) => `/products/${id}`,
    UPDATE: (id: string) => `/products/${id}`,
    DELETE: (id: string) => `/products/${id}`,
    CATEGORIES: '/products/categories',
    BY_CATEGORY: (category: string) => `/products/category/${category}`,
  },
  
  // Cart endpoints
  CARTS: {
    BASE: '/carts',
    DETAILS: (id: string) => `/carts/${id}`,
    UPDATE: (id: string) => `/carts/${id}`,
    DELETE: (id: string) => `/carts/${id}`,
    USER_CARTS: (userId: string) => `/carts/user/${userId}`,
    ADD_PRODUCT: (cartId: string) => `/carts/${cartId}/products`,
    UPDATE_PRODUCT: (cartId: string, productId: string) => `/carts/${cartId}/products/${productId}`,
    REMOVE_PRODUCT: (cartId: string, productId: string) => `/carts/${cartId}/products/${productId}`,
    COMPLETE: (cartId: string) => `/carts/${cartId}/complete`,
    
    // Novos endpoints para a API backend
    BACKEND_BASE: environment.apiUrl + 'Carts',
    CREATE_BACKEND: environment.apiUrl + 'Carts',
    LIST_BACKEND: environment.apiUrl + 'Carts',
    DETAILS_BACKEND: (id: string) => `${environment.apiUrl}Carts/${id}`,
    UPDATE_BACKEND: (id: string) => `${environment.apiUrl}Carts/${id}`,
    DELETE_BACKEND: (id: string) => `${environment.apiUrl}Carts/${id}`,
    CALCULATE_DISCOUNT: environment.apiUrl + 'Carts/CalculateDiscount'
  },
  
  // Order endpoints
  ORDERS: {
    BASE: '/orders',
    USER_ORDERS: (userId: string) => `/orders/user/${userId}`,
    DETAILS: (id: string) => `/orders/${id}`,
  },
  
  // Add more endpoint groups here as needed
};

/**
 * API headers configuration
 */
export const API_HEADERS = {
  CONTENT_TYPE: 'Content-Type',
  ACCEPT: 'Accept',
  AUTHORIZATION: 'Authorization',
};

/**
 * API content types
 */
export const API_CONTENT_TYPES = {
  JSON: 'application/json',
  FORM: 'application/x-www-form-urlencoded',
  MULTIPART: 'multipart/form-data',
};

/**
 * API configuration object
 */
export const API_CONFIG = {
  BASE_URL: environment.apiUrl,
  TIMEOUT: 30000, // 30 seconds timeout for requests
  RETRY_COUNT: 3, // Number of times to retry failed requests
  ENDPOINTS: API_ENDPOINTS,
  HEADERS: API_HEADERS,
  CONTENT_TYPES: API_CONTENT_TYPES,
}; 