using FluentValidation;
using StevenAccelokaAPI.Application.Queries;

namespace StevenAccelokaAPI.Application.Validators
{
    public class GetBookedTicketDetailsValidator : AbstractValidator<GetBookedTicketDetailsQuery>
    {
        public GetBookedTicketDetailsValidator()
        {
            RuleFor(x => x.BookedTicketId)
                .GreaterThan(0)
                .WithMessage("Booked Ticket ID must be greater than 0.");
        }
    }
}
