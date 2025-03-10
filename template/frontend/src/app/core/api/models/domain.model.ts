/**
 * Domain Models
 * 
 * This file contains domain models that represent the core business entities.
 * These models are used throughout the application and are mapped from API responses.
 */

/**
 * User Domain Model
 */
export interface User {
  id: string;
  email: string;
  name: string;
  phone: string;
  role: UserRole;
  status: UserStatus;
}

/**
 * User Settings
 */
export interface UserSettings {
  theme?: string;
  language?: string;
  notifications?: boolean;
  [key: string]: any;
}

/**
 * User Role Enum
 */
export enum UserRole {
  None = 'None',
  Customer = 'Customer',
  Manager = 'Manager',
  Admin = 'Admin'
}

/**
 * User Status Enum
 */
export enum UserStatus {
  Active = 0,
  Inactive = 1,
  Suspended = 2,
  Pending = 3
}

/**
 * Authentication Token Model
 */
export interface AuthToken {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  tokenType: string;
}

/**
 * Pagination Model
 */
export interface Pagination {
  totalItems: number;
  currentPage: number;
  pageSize: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
}

/**
 * Sort Option
 */
export interface SortOption {
  field: string;
  direction: 'asc' | 'desc';
}

/**
 * Product Domain Model
 */
export interface Product {
  id: string;
  name: string;
  price: number;
  description: string;
  category: string;
  sku: string;
  stockQuantity: number;
  status: string;
  branchExternalId: string;
  branchName: string;
  createdAt: string;
  updatedAt: string | null;
}

/**
 * Cart Item - Product in cart with quantity
 */
export interface CartItem {
  product: Product;
  quantity: number;
}

/**
 * Shopping Cart
 */
export interface ShoppingCart {
  id?: string;
  userId: string;
  items: CartItem[];
  createdAt: Date;
  updatedAt: Date;
  isCompleted: boolean;
  totalPrice: number; // Computed property
}

/**
 * Order - Completed cart/purchase
 */
export interface Order {
  id: string;
  cartId: string;
  userId: string;
  items: CartItem[];
  totalPrice: number;
  orderDate: Date;
  status: OrderStatus;
}

/**
 * Order Status
 */
export enum OrderStatus {
  COMPLETED = 'COMPLETED',
  PROCESSING = 'PROCESSING',
  CANCELLED = 'CANCELLED'
}

/**
 * Address for user
 */
export interface Address {
  city: string;
  street: string;
  number: number;
  zipcode: string;
  geolocation?: GeoLocation;
}

/**
 * Geographic coordinates
 */
export interface GeoLocation {
  lat: string;
  long: string;
} 