using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.DeleteCart
{
    public class DeleteCartCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
