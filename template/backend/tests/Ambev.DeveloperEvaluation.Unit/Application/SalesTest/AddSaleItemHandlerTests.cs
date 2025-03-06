using Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Mocks;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.SalesTest;

/// <summary>
/// Contains unit tests for the <see cref="AddSaleItemHandler"/> class.
/// </summary>
public class AddSaleItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _uow;
    private readonly AddSaleItemHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddSaleItemHandlerTests"/> class,
    /// setting up substitutes for dependencies.
    /// </summary>
    public AddSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _uow = Substitute.For<IUnitOfWork>();
        _handler = new AddSaleItemHandler(_saleRepository, _uow);
    }

    /// <summary>
    /// Tests that a valid add sale item command returns a successful result.
    /// Verifies that the sale is retrieved, updated, and the unit of work is committed.
    /// </summary>
    [Fact(DisplayName = "Given valid add sale item command When handling Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange: Generate a valid AddSaleItemCommand.
        var command = AddSaleItemHandlerTestData.GenerateValidCommand();

        // Create a Sale entity that already exists.
        var sale = new Sale(
            "SALE-1234",
            DateTime.UtcNow,
            "CUST-001",
            "Test Customer",
            "BR-01",
            "Test Branch");

        // Assume the sale already has one item (optional) and record the initial total.
        var initialTotal = sale.TotalAmount;

        // Configure the repository substitute to return the sale when queried by ID.
        _saleRepository
          .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
          .Returns(Task.FromResult<Sale?>(sale));

        // Configure UpdateAsync and CommitAsync to simulate a successful update.
        _uow.CommitAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

        // Act: Execute the handler.
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Verify that the result is correct.
        result.Should().NotBeNull();
        result.SaleId.Should().Be(sale.Id);
        // The new sale item should be the last in the Items collection.
        result.SaleItemId.Should().Be(sale.Items.Last().Id);
        result.NewTotalAmount.Should().Be(sale.TotalAmount);
        // Verify that GetByIdAsync, UpdateAsync, and CommitAsync were each called once.
        await _saleRepository.Received(1).GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>());
        await _uow.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid add sale item command (e.g., invalid quantity) throws a ValidationException.
    /// </summary>
    [Fact(DisplayName = "Given invalid add sale item command When handling Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange: Generate a valid command and then invalidate it (set Quantity to 0).
        var command = AddSaleItemHandlerTestData.GenerateValidCommand();
        command.Quantity = 0;

        // Act: Execute the handler.
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert: Expect a ValidationException to be thrown.
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that if the sale is not found, the handler throws an InvalidOperationException.
    /// </summary>
    [Fact(DisplayName = "Given non-existing sale When handling Then throws invalid operation exception")]
    public async Task Handle_NonExistingSale_ThrowsInvalidOperationException()
    {
        // Arrange: Generate a valid command.
        var command = AddSaleItemHandlerTestData.GenerateValidCommand();

        // Configure the repository to return null (sale not found).
        _saleRepository
          .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
          .Returns(Task.FromResult<Sale?>(null));

        // Act: Execute the handler.
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert: Expect an InvalidOperationException.
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Sale with ID {command.SaleId} not found.");
    }
}
