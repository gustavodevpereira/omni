using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.SalesTest;

/// <summary>
/// Contains unit tests for the <see cref="CancelSaleHandler"/> class.
/// </summary>
public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _uow;
    private readonly CancelSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSaleHandlerTests"/> class.
    /// Sets up substitutes for dependencies.
    /// </summary>
    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _uow = Substitute.For<IUnitOfWork>();
        _handler = new CancelSaleHandler(_saleRepository, _uow);
    }

    /// <summary>
    /// Tests that a valid cancel sale command successfully cancels the sale.
    /// </summary>
    [Fact(DisplayName = "Given valid cancel sale command When handling Then cancels the sale")]
    public async Task Handle_ValidRequest_CancelsSale()
    {
        // Arrange: Create a Sale entity letting the domain generate its ID.
        var sale = new Sale(
            "SALE-001",
            DateTime.UtcNow,
            "CUST-001",
            "Test Customer",
            "BR-001",
            "Test Branch");

        // Configure the repository substitute to return this sale when GetByIdAsync is called
        // using the sale's generated ID.
        _saleRepository
            .GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Sale?>(sale));

        // Prepare a valid cancel sale command using the sale's actual generated ID.
        var command = new CancelSaleCommand
        {
            SaleId = sale.Id
        };

        // Act: Execute the cancel sale command.
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Verify that the sale's status is now Cancelled,
        // and the result contains the same sale ID along with a successful cancellation flag.
        sale.Status.Should().Be(SaleStatus.Cancelled);
        result.IsCancelled.Should().BeTrue();
        result.SaleId.Should().Be(sale.Id);
        await _uow.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid command (e.g., empty SaleId) throws a ValidationException.
    /// </summary>
    [Fact(DisplayName = "Given invalid cancel sale command When handling Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange: invalid command (empty SaleId).
        var command = new CancelSaleCommand
        {
            SaleId = Guid.Empty
        };

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that if the sale does not exist, the handler throws an InvalidOperationException.
    /// </summary>
    [Fact(DisplayName = "Given non-existing sale When handling Then throws invalid operation exception")]
    public async Task Handle_NonExistingSale_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = new CancelSaleCommand
        {
            SaleId = Guid.NewGuid()
        };

        // Sale not found
        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Sale?>(null));

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Sale with ID {command.SaleId} not found.");
    }
}
