using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StevenAccelokaAPI.Application.Commands;
using StevenAccelokaAPI.Application.Queries;
using StevenAccelokaAPI.Models.DTOs;

namespace StevenAccelokaAPI.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class BookedTicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookedTicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("get-booked-ticket/{bookedTicketId}")]
        public async Task<ActionResult<List<BookedTicketCategoryDto>>> GetBookedTicketDetails(int bookedTicketId)
        {
            var result = await _mediator.Send(new GetBookedTicketDetailsQuery { BookedTicketId = bookedTicketId });
            return Ok(result);
        }


        [HttpDelete("revoke-ticket/{bookedTicketId}/{ticketCode}/{qty}")]
        public async Task<IActionResult> RevokeTicket(int bookedTicketId, string ticketCode, int qty)
        {
            var command = new RevokeTicketCommand
            {
                BookedTicketId = bookedTicketId,
                TicketCode = ticketCode,
                Quantity = qty
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("edit-booked-ticket/{bookedTicketId}")]
        public async Task<IActionResult> EditBookedTicket(int bookedTicketId, [FromBody] EditBookedTicketCommand command)
        {
            if (command == null)
            {
                return BadRequest(new { error = "Invalid request body." });
            }

            command.BookedTicketId = bookedTicketId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("get-all-booked-ids")]
        public async Task<IActionResult> GetAllBookingIds()
        {
            var result = await _mediator.Send(new GetBookedTicketIdsQuery());
            return Ok(result);
        }

    }

}

//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using StevenAccelokaAPI.Application.Commands;
//using StevenAccelokaAPI.Application.Handlers;

//namespace StevenAccelokaAPI.Controllers
//{
//    [ApiController]
//    [Route("api/v1/booked-tickets")]
//    public class BookedTicketsController : ControllerBase
//    {
//        private readonly IMediator _mediator;

//        public BookedTicketsController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        [HttpGet("{bookedTicketId}")]
//        public async Task<IActionResult> GetBookedTicketDetails(int bookedTicketId)
//        {
//            var query = new GetBookedTicketQuery(bookedTicketId);
//            var result = await _mediator.Send(query);
//            return result != null ? Ok(result) : NotFound(new { message = "Booked ticket ID not found" });
//        }

//        [HttpDelete("{bookedTicketId}/{kodeTicket}/{qty}")]
//        public async Task<IActionResult> RevokeTicket(int bookedTicketId, string kodeTicket, int qty)
//        {
//            var command = new RevokeTicketCommand(bookedTicketId, kodeTicket, qty);
//            var result = await _mediator.Send(command);
//            return result.Success ? Ok(result.Data) : StatusCode(result.StatusCode, new { message = result.Message });
//        }

//        [HttpPut("{bookedTicketId}")]
//        public async Task<IActionResult> EditBookedTicket(int bookedTicketId, [FromBody] EditBookedTicketCommand command)
//        {
//            if (command == null || command.Tickets == null || command.Tickets.Count == 0)
//            {
//                return BadRequest(new { message = "Request body tidak boleh kosong dan harus berisi tiket untuk diperbarui." });
//            }

//            command.BookedTicketId = bookedTicketId;
//            var response = await _mediator.Send(command);
//            return response.Success ? Ok(response.Data) : StatusCode(response.StatusCode, new { message = response.Message });
//        }
//    }
//}
