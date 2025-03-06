using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StevenAccelokaAPI.Models;
using StevenAccelokaAPI.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

public class GetAvailableTicketsQuery : IRequest<object>
{
    public string? CategoryName { get; set; }
    public string? TicketCode { get; set; }
    public string? TicketName { get; set; }
    public decimal? MaxPrice { get; set; }
    public DateTime? MinEventDate { get; set; }
    public DateTime? MaxEventDate { get; set; }
    public string? OrderBy { get; set; } = "ticketCode";
    public string? OrderState { get; set; } = "asc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAvailableTicketsQueryHandler : IRequestHandler<GetAvailableTicketsQuery, object>
{
    private readonly ApplicationDbContext _context;

    public GetAvailableTicketsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object> Handle(GetAvailableTicketsQuery request, CancellationToken cancellationToken)
    {
        // ✅ Validasi Request: Jika page atau pageSize tidak valid
        if (request.Page < 1 || request.PageSize < 1)
        {
            return new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Invalid Pagination Parameters",
                Detail = "Page and PageSize must be greater than zero.",
                Type = "https://example.com/probs/invalid-pagination"
            };
        }

        var query = _context.Tickets.Include(t => t.TicketCategory).AsQueryable();

        if (!string.IsNullOrEmpty(request.CategoryName))
            query = query.Where(t => t.TicketCategory.Name.Contains(request.CategoryName));

        if (!string.IsNullOrEmpty(request.TicketCode))
            query = query.Where(t => t.TicketCode.Contains(request.TicketCode));

        if (!string.IsNullOrEmpty(request.TicketName))
            query = query.Where(t => t.TicketName.Contains(request.TicketName));

        if (request.MaxPrice.HasValue)
            query = query.Where(t => t.Price <= request.MaxPrice.Value);

        if (request.MinEventDate.HasValue)
            query = query.Where(t => t.EventDate >= request.MinEventDate.Value);

        if (request.MaxEventDate.HasValue)
            query = query.Where(t => t.EventDate <= request.MaxEventDate.Value);

        int totalTickets = await query.CountAsync(cancellationToken);

        // ✅ Jika tidak ada tiket yang tersedia, gunakan ProblemDetails
        if (totalTickets == 0)
        {
            throw new ProblemDetailsException(new ProblemDetails
            {
                Type = "https://example.com/probs/no-tickets-found",
                Title = "No Available Tickets",
                Status = 404,
                Detail = "No tickets match the given filters."
            });
        }

        bool isAscending = request.OrderState?.ToLower() == "asc";
        query = request.OrderBy?.ToLower() switch
        {
            "ticketname" => isAscending ? query.OrderBy(t => t.TicketName) : query.OrderByDescending(t => t.TicketName),
            "categoryname" => isAscending ? query.OrderBy(t => t.TicketCategory.Name) : query.OrderByDescending(t => t.TicketCategory.Name),
            "price" => isAscending ? query.OrderBy(t => t.Price) : query.OrderByDescending(t => t.Price),
            "eventdate" => isAscending ? query.OrderBy(t => t.EventDate) : query.OrderByDescending(t => t.EventDate),
            _ => query.OrderBy(t => t.TicketCode),
        };

        var tickets = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new TicketDto
            {
                EventDate = t.EventDate,
                Quota = t.Quota,
                TicketCode = t.TicketCode,
                TicketName = t.TicketName,
                CategoryName = t.TicketCategory.Name,
                Price = t.Price
            })
            .ToListAsync(cancellationToken);

        return new
        {
            Success = true,
            Message = "Available tickets retrieved successfully",
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalTickets / request.PageSize),
            Data = tickets,
            TotalTickets = totalTickets,
        };
    }
}
