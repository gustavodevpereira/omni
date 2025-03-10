import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { ProductService } from '../../services/product.service';
import { Product } from '../../../../core/api/models/domain.model';
import { CartService } from '../../../cart/services/cart.service';
import { NotificationService } from '../../../../core/services/notification.service';

/**
 * ProductDetailComponent
 * 
 * Shows detailed information about a product and allows adding to cart
 */
@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [
    CommonModule,
    SharedModule
  ],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit, OnDestroy {
  product: Product | null = null;
  isLoading = true;
  quantity = 1;
  
  private readonly destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    private cartService: CartService,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
    // Get product ID from route parameters
    this.route.paramMap
      .pipe(takeUntil(this.destroy$))
      .subscribe(params => {
        const productId = params.get('id');
        if (productId) {
          this.loadProduct(productId);
        } else {
          this.router.navigate(['/products']);
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Load product details by ID
   */
  loadProduct(productId: string): void {
    this.isLoading = true;
    
    this.productService.getProductById(productId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (product) => {
          this.product = product;
          this.isLoading = false;
        },
        error: (error) => {
          this.isLoading = false;
          this.notificationService.error('Product not found');
          this.router.navigate(['/products']);
        }
      });
  }

  /**
   * Decrease quantity (minimum 1)
   */
  decreaseQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  /**
   * Increase quantity (maximum 10 for demo purposes)
   */
  increaseQuantity(): void {
    if (this.quantity < 10) {
      this.quantity++;
    }
  }

  /**
   * Add product to cart with selected quantity
   */
  addToCart(): void {
    if (!this.product) return;
    
    this.cartService.addToCart(this.product, this.quantity);
    this.notificationService.success(`Added ${this.quantity} ${this.quantity > 1 ? 'items' : 'item'} to cart`);
  }

  /**
   * Navigate back to products list
   */
  goBack(): void {
    this.router.navigate(['/products']);
  }
} 