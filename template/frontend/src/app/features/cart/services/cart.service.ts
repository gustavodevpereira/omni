import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Product } from '../../../core/api/models/domain.model';
import { NotificationService } from '../../../core/services/notification.service';

/**
 * Cart item interface
 */
export interface CartItem {
  product: Product;
  quantity: number;
}

/**
 * Cart interface
 */
export interface Cart {
  items: CartItem[];
  totalItems: number;
  totalPrice: number;
}

/**
 * Cart Service
 * 
 * Domain service that handles shopping cart and checkout functionality.
 * Manages the user's current cart, cached in memory and localStorage,
 * and provides methods for cart operations and checkout.
 */
@Injectable({
  providedIn: 'root'
})
export class CartService {
  // Storage key
  private readonly CART_STORAGE_KEY = 'shopping_cart';
  
  // Cart state
  private cartSubject = new BehaviorSubject<Cart>({ items: [], totalItems: 0, totalPrice: 0 });
  public cart$ = this.cartSubject.asObservable();
  
  constructor(
    private notificationService: NotificationService
  ) { 
    this.loadCart();
  }
  
  /**
   * Load cart from localStorage
   */
  private loadCart(): void {
    const storedCart = localStorage.getItem(this.CART_STORAGE_KEY);
    
    if (storedCart) {
      try {
        const parsedCart = JSON.parse(storedCart) as Cart;
        this.cartSubject.next(parsedCart);
      } catch (error) {
        console.error('Error parsing stored cart:', error);
        this.initializeEmptyCart();
      }
    } else {
      this.initializeEmptyCart();
    }
  }
  
  /**
   * Initialize empty cart
   */
  private initializeEmptyCart(): void {
    const emptyCart: Cart = {
      items: [],
      totalItems: 0,
      totalPrice: 0
    };
    
    this.cartSubject.next(emptyCart);
    this.saveCart(emptyCart);
  }
  
  /**
   * Save cart to localStorage
   */
  private saveCart(cart: Cart): void {
    localStorage.setItem(this.CART_STORAGE_KEY, JSON.stringify(cart));
  }
  
  /**
   * Get the current cart
   */
  getCart(): Observable<Cart> {
    return this.cart$;
  }
  
  /**
   * Add product to cart
   */
  addToCart(product: Product, quantity: number = 1): void {
    const currentCart = this.cartSubject.value;
    const existingItemIndex = currentCart.items.findIndex(item => item.product.id === product.id);
    
    if (existingItemIndex !== -1) {
      // Update quantity if product already exists in cart
      currentCart.items[existingItemIndex].quantity += quantity;
    } else {
      // Add new item
      currentCart.items.push({ product, quantity });
    }
    
    // Update totals
    this.updateCartTotals(currentCart);
    
    // Save and update state
    this.saveCart(currentCart);
    this.cartSubject.next({ ...currentCart });
    
    this.notificationService.success(`Added ${product.name} to cart`);
  }
  
  /**
   * Update item quantity
   */
  updateQuantity(productId: string, quantity: number): void {
    const currentCart = this.cartSubject.value;
    const itemIndex = currentCart.items.findIndex(item => item.product.id === productId);
    
    if (itemIndex === -1) {
      return;
    }
    
    if (quantity <= 0) {
      // Remove item if quantity is 0 or negative
      this.removeFromCart(productId);
      return;
    }
    
    // Update quantity
    currentCart.items[itemIndex].quantity = quantity;
    
    // Update totals
    this.updateCartTotals(currentCart);
    
    // Save and update state
    this.saveCart(currentCart);
    this.cartSubject.next({ ...currentCart });
  }
  
  /**
   * Remove item from cart
   */
  removeFromCart(productId: string): void {
    const currentCart = this.cartSubject.value;
    
    // Filter out the item
    currentCart.items = currentCart.items.filter(item => item.product.id !== productId);
    
    // Update totals
    this.updateCartTotals(currentCart);
    
    // Save and update state
    this.saveCart(currentCart);
    this.cartSubject.next({ ...currentCart });
    
    this.notificationService.info('Item removed from cart');
  }
  
  /**
   * Clear cart
   */
  clearCart(): void {
    this.initializeEmptyCart();
    this.notificationService.info('Cart cleared');
  }
  
  /**
   * Update cart totals
   */
  private updateCartTotals(cart: Cart): void {
    cart.totalItems = cart.items.reduce((total, item) => total + item.quantity, 0);
    cart.totalPrice = cart.items.reduce((total, item) => total + (item.product.price * item.quantity), 0);
  }
} 