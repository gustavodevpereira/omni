# Backend Project Structure Guide

## Project Structure

```
src/
├── Ambev.DeveloperEvaluation.WebApi/        # API Layer
├── Ambev.DeveloperEvaluation.Application/    # Application Layer
├── Ambev.DeveloperEvaluation.Domain/         # Domain Layer
├── Ambev.DeveloperEvaluation.ORM/           # Infrastructure Layer
├── Ambev.DeveloperEvaluation.Common/        # Shared Utilities
└── Ambev.DeveloperEvaluation.IoC/           # Dependency Injection
```

### Domain Layer (`Domain/`)
The core business logic and rules, completely independent of external concerns:

- **Entities**: Core business objects
  - `User.cs` - User domain entity
  - `Product.cs` - Product domain entity
  - `Cart.cs` - Shopping cart entity

- **Repositories**: Interface definitions
  - `IUserRepository.cs`
  - `IProductRepository.cs`
  - `ICartRepository.cs`

- **Specifications**: Business rules
  - `ISpecification.cs` - Specification pattern interface
  - Domain-specific specifications

### Application Layer (`Application/`)
Contains application business logic and orchestration:

- **Use Cases**: Organized by feature
  ```
  Users/
  ├── CreateUser/
  │   ├── CreateUserCommand.cs
  │   ├── CreateUserHandler.cs
  │   └── CreateUserProfile.cs
  ├── GetUser/
  └── DeleteUser/
  ```

- **Common**: Shared application concerns
  - Exception handling
  - Validation
  - Behaviors

### Infrastructure Layer (`ORM/`)
Database and external service implementations:

- **Context**: Entity Framework configuration
  - `DefaultContext.cs` - Main DB context

- **Repositories**: Implementation of domain repositories
  - `UserRepository.cs`
  - `ProductRepository.cs`
  - `CartRepository.cs`

- **UnitOfWork**: Transaction management
  ```csharp
  public class UnitOfWork : IUnitOfWork
  {
      private readonly DbContext _context;
      private readonly IMediator _mediator;
      // Implementation details...
  }
  ```

### Web API Layer (`WebApi/`)
The presentation layer handling HTTP requests:

- **Features**: Organized by domain feature
  ```
  Features/
  ├── Auth/
  │   └── AuthController.cs
  ├── Products/
  │   └── ProductsController.cs
  └── Cart/
      └── CartsController.cs
  ```

- **Common**: API-specific shared components
  - Base controller
  - Middleware
  - Filters

### Common Layer (`Common/`)
Shared utilities and cross-cutting concerns:

- Security utilities
- Logging configuration
- Health checks
- Validation helpers

### IoC Layer (`IoC/`)
Dependency injection and service registration:

- Module initializers
- Service collection extensions
- Configuration helpers

## Architecture Patterns

### CQRS with MediatR
The application uses Command Query Responsibility Segregation:
- Commands: State-changing operations
- Queries: Read-only operations
- MediatR for in-process messaging

### Repository Pattern
- Abstract data access
- Domain-focused interfaces
- Implementation in ORM layer

### Unit of Work
- Coordinates transactions
- Manages domain events
- Ensures data consistency