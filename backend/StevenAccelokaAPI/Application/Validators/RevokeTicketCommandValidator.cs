﻿using FluentValidation;
using StevenAccelokaAPI.Application.Commands;

namespace StevenAccelokaAPI.Application.Validators
{
    public class RevokeTicketCommandValidator : AbstractValidator<RevokeTicketCommand>
    {
        public RevokeTicketCommandValidator()
        {
            RuleFor(x => x.BookedTicketId)
                .GreaterThan(0)
                .WithMessage("BookedTicketId must be greater than 0.");

            RuleFor(x => x.TicketCode)
                .NotEmpty()
                .WithMessage("TicketCode is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be at least 1.");
        }
    }

}
