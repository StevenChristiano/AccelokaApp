using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using StevenAccelokaAPI.Application.Queries;
using StevenAccelokaAPI.Application.Commands;

[Route("api/v1/")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-available-ticket")]
    public async Task<IActionResult> GetAvailableTickets([FromQuery] GetAvailableTicketsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("book-ticket")]
    public async Task<IActionResult> BookTicket([FromBody] BookTicketCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
