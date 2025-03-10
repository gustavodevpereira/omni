import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { SharedModule } from '../../../../shared/shared.module';
import { ProductService } from '../../services/product.service';
import { Product } from '../../../../core/api/models/domain.model';
import { PageEvent } from '@angular/material/paginator';
import { CartService } from '../../../cart/services/cart.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { MatDialog } from '@angular/material/dialog';
import { CartModalComponent } from '../../../cart/components/cart-modal/cart-modal.component';
import { MatBadgeModule } from '@angular/material/badge';

/**
 * ProductListComponent
 * 
 * Displays a grid of products with pagination capabilities.
 */
@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule,
    SharedModule,
    MatBadgeModule
  ],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit, OnDestroy {
  products: Product[] = [];
  
  isLoading = false;
  
  // Pagination
  currentPage = 0;
  pageSize = 6; // Default page size
  totalProducts = 0;
  
  // Cart
  cartItemCount = 0;
  
  private readonly destroy$ = new Subject<void>();

  constructor(
    private productService: ProductService,
    private cartService: CartService,
    private notificationService: NotificationService,
    private dialog: MatDialog,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Initial load
    this.loadProducts();
    
    // Subscribe to cart changes
    this.cartService.getCart()
      .pipe(takeUntil(this.destroy$))
      .subscribe(cart => {
        this.cartItemCount = cart.totalItems;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Load products
   */
  loadProducts(): void {
    this.isLoading = true;
    
    const pagination = {
      page: this.currentPage,
      limit: this.pageSize
    };
    
    this.productService.getProducts(pagination)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.products = response.products;
          this.totalProducts = response.total;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading products:', error);
          this.notificationService.error('Failed to load products. Please try again.');
          this.isLoading = false;
          this.products = [];
        }
      });
  }

  /**
   * Handle paginator events
   */
  onPageChange(event: PageEvent): void {
    if (this.currentPage !== event.pageIndex || this.pageSize !== event.pageSize) {
      this.currentPage = event.pageIndex;
      this.pageSize = event.pageSize;
      this.loadProducts();
      
      // Scroll to top of page when changing pages
      window.scrollTo(0, 0);
    }
  }

  /**
   * Add product to cart and show cart modal
   */
  addToCart(product: Product): void {
    this.cartService.addToCart(product);
    this.openCartModal();
  }
  
  /**
   * Open cart modal
   */
  openCartModal(): void {
    const isMobile = window.innerWidth < 768;
    
    this.dialog.open(CartModalComponent, {
      width: isMobile ? '100%' : '500px',
      maxWidth: isMobile ? '100%' : '500px',
      height: isMobile ? '100%' : 'auto',
      maxHeight: isMobile ? '100%' : '90vh',
      panelClass: isMobile ? 'mobile-cart-dialog' : '',
      autoFocus: true,
      restoreFocus: true,
      disableClose: false,
      hasBackdrop: true,
      backdropClass: 'cart-backdrop',
      ariaDescribedBy: 'cart-modal-description',
      ariaLabelledBy: 'cart-modal-title'
    });
    
    // Fix for mobile footer focus issue
    if (isMobile) {
      setTimeout(() => {
        const mobileFooter = document.querySelector('.mobile-footer');
        if (mobileFooter) {
          mobileFooter.setAttribute('aria-hidden', 'false');
          
          // Ensure no parent has aria-hidden
          let parent = mobileFooter.parentElement;
          while (parent) {
            if (parent.hasAttribute('aria-hidden')) {
              parent.setAttribute('aria-hidden', 'false');
            }
            parent = parent.parentElement;
          }
        }
      }, 300);
    }
  }
} 