using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleByNumber;

/// <summary>
/// Command for retrieving a sale by its number
/// </summary>
public record GetSaleByNumberCommand : IRequest<GetSaleByNumberResult>
{
    /// <summary>
    /// Gets the sale number
    /// </summary>
    public string SaleNumber { get; }

    /// <summary>
    /// Initializes a new instance of GetSaleByNumberCommand
    /// </summary>
    /// <param name="saleNumber">The sale number</param>
    public GetSaleByNumberCommand(string saleNumber)
    {
        SaleNumber = saleNumber;
    }
} 