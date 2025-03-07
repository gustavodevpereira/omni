# Script to separate changes into logical commits

# 1. First commit: Remove Sales-related files
git add --all "template/backend/src/Ambev.DeveloperEvaluation.Application/Sales" 
git add --all "template/backend/src/Ambev.DeveloperEvaluation.Domain/Entities/Sales"
git add --all "template/backend/src/Ambev.DeveloperEvaluation.Domain/Events/Sales"
git add --all "template/backend/src/Ambev.DeveloperEvaluation.Domain/Repositories/ISaleRepository.cs"
git add --all "template/backend/src/Ambev.DeveloperEvaluation.ORM/Repositories/SaleRepository.cs"
git add --all "template/backend/src/Ambev.DeveloperEvaluation.WebApi/Features/Sales"
git add --all "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Application/SalesTest"
git add --all "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Domain/Entities/SalesTest"
git commit -m "#10 - Remove unused sales resources"

# 2. Second commit: Add feature folders structure for Products
git add "refaactor.ps1"
git add --all "template/backend/src/Ambev.DeveloperEvaluation.WebApi/Features/Product"
git add "template/backend/src/Ambev.DeveloperEvaluation.Domain/Entities/Products/Product.cs"
git commit -m "#10 - Add feature folders structure for Products"

# 3. Third commit: Add Cart structure
git add --all "template/backend/src/Ambev.DeveloperEvaluation.Application/Carts"
git add --all "template/backend/src/Ambev.DeveloperEvaluation.Domain/Entities/Carts"
git add "template/backend/src/Ambev.DeveloperEvaluation.Domain/Enums/CartStatus.cs"
git add "template/backend/src/Ambev.DeveloperEvaluation.Domain/Repositories/ICartRepository.cs"
git add "template/backend/src/Ambev.DeveloperEvaluation.Domain/Validation/CartValidator.cs"
git add "template/backend/src/Ambev.DeveloperEvaluation.ORM/Repositories/CartRepository.cs"
git add --all "template/backend/src/Ambev.DeveloperEvaluation.WebApi/Features/Cart"
git add --all "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Application/CartsTest"
git add --all "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Domain/Entities/CartTest"
git commit -m "#10 - Add new shopping cart functionality"

# 4. Fourth commit: Update infrastructure and configuration files
git add "template/backend/src/Ambev.DeveloperEvaluation.ORM/DefaultContext.cs"
git add --all "template/backend/src/Ambev.DeveloperEvaluation.ORM/Migrations"
git add "template/backend/src/Ambev.DeveloperEvaluation.IoC/ModuleInitializers/InfrastructureModuleInitializer.cs"
git add "template/backend/src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"
git add "template/backend/src/Ambev.DeveloperEvaluation.Domain/Ambev.DeveloperEvaluation.Domain.csproj"
git add "template/backend/src/Ambev.DeveloperEvaluation.WebApi/appsettings.json"
git commit -m "#10 - Update infrastructure and configurations for new features"

# 5. Fifth commit: Reorganize and update tests
git add --all "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Application/Mocks"
git add "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Application/ProductTest/CreateProductHandlerTests.cs"
git add "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Application/ProductTest/UpdateProductHandlerTests.cs"
git add "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Application/UserTest/CreateUserHandlerTests.cs"
git add "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Domain/Entities/ProductTest/ProductTests.cs"
git add "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Domain/Entities/UserTest/UserTests.cs"
git add "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Domain/Validation/EmailValidatorTests.cs"
git add "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Domain/Validation/PasswordValidatorTests.cs"
git add "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Domain/Validation/UserValidatorTests.cs"
git add --all "template/backend/tests/Ambev.DeveloperEvaluation.Unit/Domain/Mocks"
git commit -m "#10 - Reorganize and update unit tests"

# 6. Sixth commit: Finalize refactoring with remaining files
git add .
git commit -m "#10 - Finalize refactoring with minor adjustments"

# Show status after all commits
git status