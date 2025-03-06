using FluentValidation;
using StevenAccelokaAPI.Application.Queries;

namespace StevenAccelokaAPI.Application.Validators
{
    public class GetAvailableTicketsQueryValidator : AbstractValidator<GetAvailableTicketsQuery>
    {
        public GetAvailableTicketsQueryValidator()
        {
            RuleFor(x => x.MaxPrice)
                .GreaterThan(0).WithMessage("MaxPrice must be greater than zero.")
                .When(x => x.MaxPrice.HasValue);

            RuleFor(x => x.OrderBy)
                .Must(x => new[] { "ticketCode", "price", "eventDate" }.Contains(x))
                .WithMessage("Invalid OrderBy value. Allowed: ticketCode, price, eventDate.");

            RuleFor(x => x.OrderState)
                .Must(x => new[] { "asc", "desc" }.Contains(x))
                .WithMessage("Invalid OrderState value. Allowed: asc, desc.");
        }
    }
}
