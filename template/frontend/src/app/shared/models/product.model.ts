/**
 * Product domain model
 * Implements domain logic for products with proper validation
 */
export class Product {
  constructor(
    public readonly id: string,
    public readonly name: string,
    public readonly description: string,
    public readonly sku: string,
    private readonly _price: number,
    public readonly stockQuantity: number,
    public readonly category: string,
    public readonly status: ProductStatus,
    public readonly createdAt: Date,
    public readonly updatedAt?: Date
  ) {
    this.validateProduct();
  }

  /**
   * Factory method to create a new Product instance
   */
  static create(props: ProductProps): Product {
    return new Product(
      props.id,
      props.name,
      props.description,
      props.sku,
      props.price,
      props.stockQuantity,
      props.category,
      props.status || ProductStatus.ACTIVE,
      props.createdAt || new Date(),
      props.updatedAt
    );
  }

  /**
   * Factory method to create a Product from DTO
   */
  static fromDTO(dto: ProductDTO): Product {
    return Product.create({
      id: dto.id,
      name: dto.name,
      description: dto.description,
      sku: dto.sku,
      price: dto.price,
      stockQuantity: dto.stockQuantity,
      category: dto.category,
      status: dto.status as ProductStatus,
      createdAt: new Date(dto.createdAt),
      updatedAt: dto.updatedAt ? new Date(dto.updatedAt) : undefined
    });
  }

  /**
   * Validates product properties
   * @throws Error if validation fails
   */
  private validateProduct(): void {
    if (!this.name || this.name.trim().length < 2) {
      throw new Error('Product name must be at least 2 characters');
    }

    if (this._price < 0) {
      throw new Error('Price cannot be negative');
    }

    if (this.stockQuantity < 0) {
      throw new Error('Stock quantity cannot be negative');
    }
  }

  /**
   * Gets the formatted price
   */
  get formattedPrice(): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(this._price);
  }

  /**
   * Gets the raw price value
   */
  get price(): number {
    return this._price;
  }

  /**
   * Checks if product is in stock
   */
  isInStock(): boolean {
    return this.stockQuantity > 0;
  }

  /**
   * Checks if a specific quantity of product can be added to cart
   */
  canBeAddedToCart(quantity: number): boolean {
    return quantity > 0 && this.stockQuantity >= quantity;
  }

  /**
   * Converts to plain DTO object
   */
  toDTO(): ProductDTO {
    return {
      id: this.id,
      name: this.name,
      description: this.description,
      sku: this.sku,
      price: this._price,
      stockQuantity: this.stockQuantity,
      category: this.category,
      status: this.status,
      createdAt: this.createdAt.toISOString(),
      updatedAt: this.updatedAt?.toISOString()
    };
  }
}

/**
 * Product status enum
 */
export enum ProductStatus {
  ACTIVE = 'active',
  INACTIVE = 'inactive',
  DISCONTINUED = 'discontinued'
}

/**
 * Interface for creating a Product
 */
export interface ProductProps {
  id: string;
  name: string;
  description: string;
  sku: string;
  price: number;
  stockQuantity: number;
  category: string;
  status?: ProductStatus;
  createdAt?: Date;
  updatedAt?: Date;
}

/**
 * Data Transfer Object for Product
 */
export interface ProductDTO {
  id: string;
  name: string;
  description: string;
  sku: string;
  price: number;
  stockQuantity: number;
  category: string;
  status: string;
  createdAt: string;
  updatedAt?: string;
}
