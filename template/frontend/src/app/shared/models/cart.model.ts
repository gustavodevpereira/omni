import { Product, ProductDTO } from './product.model';

/**
 * CartItem domain model
 */
export class CartItem {
  constructor(
    public readonly id: string,
    public readonly productId: string,
    private readonly _product: Product,
    private _quantity: number,
    public readonly unitPrice: number,
    public readonly discount: number
  ) {
    this.validateCartItem();
  }

  /**
   * Factory method to create a new CartItem instance
   */
  static create(props: CartItemProps): CartItem {
    return new CartItem(
      props.id,
      props.productId,
      props.product,
      props.quantity,
      props.unitPrice,
      props.discount || 0
    );
  }

  /**
   * Factory method to create a CartItem from DTO
   */
  static fromDTO(dto: CartItemDTO): CartItem {
    return CartItem.create({
      id: dto.id,
      productId: dto.productId,
      product: Product.fromDTO(dto.product),
      quantity: dto.quantity,
      unitPrice: dto.unitPrice,
      discount: dto.discount
    });
  }

  /**
   * Validates cart item properties
   * @throws Error if validation fails
   */
  private validateCartItem(): void {
    if (this._quantity <= 0) {
      throw new Error('Quantity must be greater than zero');
    }

    if (this.unitPrice < 0) {
      throw new Error('Unit price cannot be negative');
    }

    if (this.discount < 0 || this.discount > this.unitPrice) {
      throw new Error('Discount cannot be negative or greater than unit price');
    }
  }

  /**
   * Gets the product
   */
  get product(): Product {
    return this._product;
  }

  /**
   * Gets the quantity
   */
  get quantity(): number {
    return this._quantity;
  }

  /**
   * Updates the quantity
   * @throws Error if new quantity is invalid
   */
  updateQuantity(newQuantity: number): void {
    if (newQuantity <= 0) {
      throw new Error('Quantity must be greater than zero');
    }
    
    if (newQuantity > this._product.stockQuantity) {
      throw new Error('Quantity cannot exceed available stock');
    }
    
    this._quantity = newQuantity;
  }

  /**
   * Calculates the total amount for this item
   */
  get totalAmount(): number {
    return (this.unitPrice - this.discount) * this._quantity;
  }

  /**
   * Converts to plain DTO object
   */
  toDTO(): CartItemDTO {
    return {
      id: this.id,
      productId: this.productId,
      product: this._product.toDTO(),
      quantity: this._quantity,
      unitPrice: this.unitPrice,
      discount: this.discount,
      totalAmount: this.totalAmount
    };
  }
}

/**
 * Cart domain model
 */
export class Cart {
  constructor(
    public readonly id: string,
    public readonly createdOn: Date,
    public readonly customerExternalId: string,
    public readonly customerName: string,
    public readonly branchExternalId: string,
    public readonly branchName: string,
    public readonly status: CartStatus,
    private readonly _items: CartItem[]
  ) {
    this.validateCart();
  }

  /**
   * Factory method to create a new Cart instance
   */
  static create(props: CartProps): Cart {
    return new Cart(
      props.id,
      props.createdOn || new Date(),
      props.customerExternalId,
      props.customerName,
      props.branchExternalId,
      props.branchName,
      props.status || CartStatus.OPEN,
      props.items || []
    );
  }

  /**
   * Factory method to create a Cart from DTO
   */
  static fromDTO(dto: CartDTO): Cart {
    return Cart.create({
      id: dto.id,
      createdOn: new Date(dto.createdOn),
      customerExternalId: dto.customerExternalId,
      customerName: dto.customerName,
      branchExternalId: dto.branchExternalId,
      branchName: dto.branchName,
      status: dto.status as CartStatus,
      items: dto.products.map(p => CartItem.fromDTO(p))
    });
  }

  /**
   * Validates cart properties
   * @throws Error if validation fails
   */
  private validateCart(): void {
    if (!this.id) {
      throw new Error('Cart ID is required');
    }

    if (!this.customerExternalId) {
      throw new Error('Customer ID is required');
    }
  }

  /**
   * Gets cart items
   */
  get items(): CartItem[] {
    return [...this._items];
  }

  /**
   * Calculates the total amount for the entire cart
   */
  get totalAmount(): number {
    return this._items.reduce((sum, item) => sum + item.totalAmount, 0);
  }

  /**
   * Adds an item to the cart
   * @throws Error if the item already exists
   */
  addItem(item: CartItem): void {
    if (this.status !== CartStatus.OPEN) {
      throw new Error('Cannot modify a cart that is not open');
    }
    
    const existingItemIndex = this._items.findIndex(i => i.productId === item.productId);
    
    if (existingItemIndex !== -1) {
      throw new Error('Product already in cart. Use updateItemQuantity instead.');
    }
    
    this._items.push(item);
  }

  /**
   * Updates the quantity of an existing cart item
   * @throws Error if the item does not exist
   */
  updateItemQuantity(productId: string, newQuantity: number): void {
    if (this.status !== CartStatus.OPEN) {
      throw new Error('Cannot modify a cart that is not open');
    }
    
    const item = this._items.find(i => i.productId === productId);
    
    if (!item) {
      throw new Error('Item not found in cart');
    }
    
    item.updateQuantity(newQuantity);
  }

  /**
   * Removes an item from the cart
   * @throws Error if the item does not exist
   */
  removeItem(productId: string): void {
    if (this.status !== CartStatus.OPEN) {
      throw new Error('Cannot modify a cart that is not open');
    }
    
    const initialLength = this._items.length;
    const index = this._items.findIndex(item => item.productId === productId);
    
    if (index !== -1) {
      this._items.splice(index, 1);
    }
    
    if (initialLength === this._items.length) {
      throw new Error('Item not found in cart');
    }
  }

  /**
   * Checks if the cart is empty
   */
  isEmpty(): boolean {
    return this._items.length === 0;
  }

  /**
   * Gets the number of items in the cart
   */
  get itemCount(): number {
    return this._items.length;
  }

  /**
   * Gets the total quantity of all items in the cart
   */
  get totalQuantity(): number {
    return this._items.reduce((sum, item) => sum + item.quantity, 0);
  }

  /**
   * Converts to plain DTO object
   */
  toDTO(): CartDTO {
    return {
      id: this.id,
      createdOn: this.createdOn.toISOString(),
      customerExternalId: this.customerExternalId,
      customerName: this.customerName,
      branchExternalId: this.branchExternalId,
      branchName: this.branchName,
      status: this.status,
      products: this._items.map(item => item.toDTO()),
      totalAmount: this.totalAmount
    };
  }
}

/**
 * Cart status enum
 */
export enum CartStatus {
  OPEN = 'open',
  CHECKOUT = 'checkout',
  COMPLETED = 'completed',
  CANCELLED = 'cancelled'
}

/**
 * Interface for creating a CartItem
 */
export interface CartItemProps {
  id: string;
  productId: string;
  product: Product;
  quantity: number;
  unitPrice: number;
  discount?: number;
}

/**
 * Data Transfer Object for CartItem
 */
export interface CartItemDTO {
  id: string;
  productId: string;
  product: ProductDTO;
  quantity: number;
  unitPrice: number;
  discount: number;
  totalAmount: number;
}

/**
 * Interface for creating a Cart
 */
export interface CartProps {
  id: string;
  createdOn?: Date;
  customerExternalId: string;
  customerName: string;
  branchExternalId: string;
  branchName: string;
  status?: CartStatus;
  items?: CartItem[];
}

/**
 * Data Transfer Object for Cart
 */
export interface CartDTO {
  id: string;
  createdOn: string;
  customerExternalId: string;
  customerName: string;
  branchExternalId: string;
  branchName: string;
  status: string;
  products: CartItemDTO[];
  totalAmount: number;
}

/**
 * Add to cart request DTO
 */
export interface AddToCartRequest {
  productId: string;
  quantity: number;
}

/**
 * Update cart item request DTO
 */
export interface UpdateCartItemRequest {
  productId: string;
  quantity: number;
}
