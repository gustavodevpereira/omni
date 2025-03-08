import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { Cart } from '../../../shared/models/cart.model';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private endpoint = 'carts';
  private currentCartSubject = new BehaviorSubject<Cart | null>(null);
  public currentCart$ = this.currentCartSubject.asObservable();

  constructor(private apiService: ApiService) { 
    this.loadActiveCart();
  }

  private loadActiveCart(): void {
    this.getActiveCart().subscribe({
      next: cart => this.currentCartSubject.next(cart),
      error: error => console.error('Error loading cart:', error)
    });
  }

  getActiveCart(): Observable<Cart> {
    return this.apiService.get<Cart>(`${this.endpoint}/active`);
  }

  getCart(id: string): Observable<Cart> {
    return this.apiService.get<Cart>(`${this.endpoint}/${id}`);
  }

  addToCart(productId: string, quantity: number): Observable<Cart> {
    return this.apiService.post<Cart>(`${this.endpoint}/items`, { productId, quantity })
      .pipe(
        tap(cart => this.currentCartSubject.next(cart))
      );
  }

  updateCartItem(itemId: string, quantity: number): Observable<Cart> {
    return this.apiService.put<Cart>(`${this.endpoint}/items/${itemId}`, { quantity })
      .pipe(
        tap(cart => this.currentCartSubject.next(cart))
      );
  }

  removeCartItem(itemId: string): Observable<Cart> {
    return this.apiService.delete<Cart>(`${this.endpoint}/items/${itemId}`)
      .pipe(
        tap(cart => this.currentCartSubject.next(cart))
      );
  }

  clearCart(): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/clear`)
      .pipe(
        tap(() => this.currentCartSubject.next(null))
      );
  }

  checkout(): Observable<any> {
    return this.apiService.post<any>(`${this.endpoint}/checkout`, {})
      .pipe(
        tap(() => this.currentCartSubject.next(null))
      );
  }
} 