using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Mocks;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.SalesTest;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerTests"/> class,
    /// setting up the test dependencies using NSubstitute.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        // Create substitutes (mocks) for dependencies
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _uow = Substitute.For<IUnitOfWork>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _uow);
    }

    /// <summary>
    /// Tests that a valid sale creation command returns a successful response.
    /// It verifies that the repository and unit of work methods are called as expected.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange: Generate a valid CreateSaleCommand with test data.
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        // Create a Sale entity using the test command values.
        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerExternalId,
            command.CustomerName,
            command.BranchExternalId,
            command.BranchName);

        // Add each sale item from the command to the Sale entity.
        foreach (var item in command.Items)
        {
            sale.AddItem(item.ProductExternalId, item.ProductName, item.Quantity, item.UnitPrice);
        }

        // Expected result mapping.
        var expectedResult = new CreateSaleResult
        {
            Id = sale.Id
        };

        // Configure the mapper and repository substitutes.
        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(expectedResult);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(sale));
        _uow.CommitAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

        // Act: Execute the handler with the valid command.
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Verify that the result is not null, and its ID matches the sale's ID.
        result.Should().NotBeNull();
        result.Id.Should().Be(sale.Id);
        // Verify that the repository's CreateAsync method was called exactly once.
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        // Verify that the unit of work's CommitAsync method was called exactly once.
        await _uow.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that when an invalid sale command is provided, the handler throws a ValidationException.
    /// In this test, an empty SaleNumber is used to simulate an invalid command.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange: Generate a valid command and then invalidate it by setting an empty SaleNumber.
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        command.SaleNumber = string.Empty;

        // Act: Execute the handler and capture the action.
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert: Expect a ValidationException to be thrown.
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that the handler calls the AutoMapper to map the CreateSaleCommand to a Sale entity with the correct data.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps command to sale entity")]
    public async Task Handle_ValidRequest_MapsCommandToSale()
    {
        // Arrange: Generate a valid command.
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        // Create a Sale entity based on the command values.
        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerExternalId,
            command.CustomerName,
            command.BranchExternalId,
            command.BranchName);

        // Add each sale item from the command to the Sale entity.
        foreach (var item in command.Items)
        {
            sale.AddItem(item.ProductExternalId, item.ProductName, item.Quantity, item.UnitPrice);
        }

        // Configure the mapper and repository substitutes.
        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(sale));
        _uow.CommitAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

        // Act: Execute the handler.
        await _handler.Handle(command, CancellationToken.None);

        // Assert: Verify that the AutoMapper was called exactly once with a CreateSaleCommand that has the expected values.
        _mapper.Received(1).Map<Sale>(Arg.Is<CreateSaleCommand>(c =>
            c.SaleNumber == command.SaleNumber &&
            c.CustomerExternalId == command.CustomerExternalId &&
            c.CustomerName == command.CustomerName &&
            c.BranchExternalId == command.BranchExternalId &&
            c.BranchName == command.BranchName));
    }
}
