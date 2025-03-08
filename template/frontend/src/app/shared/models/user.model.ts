/**
 * User domain model
 */
export class User {
  constructor(
    public readonly email: string,
    public readonly id?: string,
    public readonly name?: string,
    public readonly role?: UserRole
  ) {
    this.validateUser();
  }

  /**
   * Factory method to create a new User instance
   */
  static create(props: UserProps): User {
    return new User(
      props.email,
      props.id,
      props.name,
      props.role
    );
  }

  /**
   * Factory method to create a User from DTO
   */
  static fromDTO(dto: UserDTO): User {
    return User.create({
      id: dto.id,
      email: dto.email,
      name: dto.name,
      role: dto.role as UserRole
    });
  }

  /**
   * Validates user properties
   * @throws Error if validation fails
   */
  private validateUser(): void {
    if (!this.email || !this.isValidEmail(this.email)) {
      throw new Error('Invalid email format');
    }
  }

  /**
   * Validates email format
   */
  private isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  /**
   * Checks if user has admin role
   */
  isAdmin(): boolean {
    return this.role === UserRole.ADMIN;
  }

  /**
   * Converts to plain DTO object
   */
  toDTO(): UserDTO {
    return {
      id: this.id,
      email: this.email,
      name: this.name,
      role: this.role
    };
  }
}

/**
 * User role enum
 */
export enum UserRole {
  ADMIN = 'admin',
  CUSTOMER = 'customer'
}

/**
 * User status enum
 */
export enum UserStatus {
  ACTIVE = 'active',
  INACTIVE = 'inactive',
  LOCKED = 'locked'
}

/**
 * Interface for creating a User
 */
export interface UserProps {
  email: string;
  id?: string;
  name?: string;
  role?: UserRole;
}

/**
 * Data Transfer Object for User
 */
export interface UserDTO {
  id?: string;
  email: string;
  name?: string;
  role?: string;
  status?: string;
  phone?: string;
}

/**
 * Authentication request DTO - matches AuthenticateUserRequest in backend
 */
export interface AuthRequest {
  email: string;
  password: string;
}

/**
 * Authentication response DTO - matches AuthenticateUserResponse in backend
 */
export interface AuthResponse {
  token: string;
  email: string;
  name: string;
  role: string;
}

/**
 * Registration request DTO - matches CreateUserRequest in backend
 */
export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  phone?: string;
  status?: UserStatus;
  role?: UserRole;
} 