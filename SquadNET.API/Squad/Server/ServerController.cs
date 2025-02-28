using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using SquadNET.Application.Squad.Admin.Commands;

/// <summary>
/// Controller for server-related operations in the Squad server.
/// </summary>
[ApiController]
[Route("server")]
public class ServerController : ControllerBase
{
    private readonly IMediator Mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerController"/> class.
    /// </summary>
    /// <param name="mediator">Mediator for handling commands.</param>
    public ServerController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Changes the availability of all actions on the server.
    /// </summary>
    [HttpPost("actions-availability")]
    public async Task<IActionResult> ForceAllActionAvailability([FromBody] ForceAllActionAvailabilityCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "All actions availability changed", result });
    }

    /// <summary>
    /// Sets the server password.
    /// </summary>
    [HttpPost("password")]
    public async Task<IActionResult> SetServerPassword([FromBody] SetServerPasswordCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Server password set", result });
    }

    /// <summary>
    /// Sets the number of reserved slots on the server.
    /// </summary>
    [HttpPost("reserved-slots")]
    public async Task<IActionResult> SetNumReservedSlots([FromBody] SetNumReservedSlotsCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Reserved slots updated", result });
    }

    /// <summary>
    /// Sets the public queue limit on the server.
    /// </summary>
    [HttpPost("public-queue-limit")]
    public async Task<IActionResult> SetPublicQueueLimit([FromBody] SetPublicQueueLimitCommand.Request request, CancellationToken cancellationToken)
    {
        string result = await Mediator.Send(request, cancellationToken);
        return Ok(new { message = "Public queue limit updated", result });
    }
}
